using Clinio.Application.Common;
using Clinio.Application.Interfaces;
using Clinio.Domain.Entities.Appointments;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clinio.Application.Features.Appointments.Commands.CreateDoctorSchedule;

public class CreateDoctorScheduleHandler(
    IApplicationDbContext db,
    ILocalizationService _localization,
    ITimeSlotGenerator _slotGenerator
) : IRequestHandler<CreateDoctorScheduleCommand, Result<int>>
{
    private const int DefaultDaysAhead = 30;

    public async Task<Result<int>> Handle(CreateDoctorScheduleCommand request, CancellationToken ct)
    {
        // check doctor exists in that clinic
        var doctor = await db.Doctors
            .FirstOrDefaultAsync(d => d.Id == request.DoctorId && d.ClinicId == request.ClinicId, ct);

        if (doctor is null)
            return Result<int>.Failure(new ResultError("Identity.UserNotFound", _localization.Get("Identity.UserNotFound")));

        // check no duplicate schedule for same day
        var exists = await db.DoctorSchedules
            .AnyAsync(s =>
                s.DoctorId == request.DoctorId &&
                s.ClinicId == request.ClinicId &&
                s.DayOfWeek == request.DayOfWeek, ct);

        if (exists)
            return Result<int>.Failure(new ResultError("ScheduleErrors.AlreadyExist", _localization.Get("ScheduleErrors.AlreadyExist")));

        var schedule = new DoctorSchedule
        {
            DoctorId = request.DoctorId,
            ClinicId = request.ClinicId,
            DayOfWeek = request.DayOfWeek,
            StartTime = request.StartTime,
            EndTime = request.EndTime,
            SlotDurationMinutes = request.SlotDurationMinutes,
            IsActive = true
        };

        db.DoctorSchedules.Add(schedule);
        await db.SaveChangesAsync(ct); // need schedule.Id before generating slots

        var newSlots = _slotGenerator.GenerateSlots(schedule, DefaultDaysAhead, new HashSet<(DateOnly, TimeOnly)>());

        db.TimeSlots.AddRange(newSlots);
        await db.SaveChangesAsync(ct);

        return Result<int>.Success(schedule.Id);
    }
}