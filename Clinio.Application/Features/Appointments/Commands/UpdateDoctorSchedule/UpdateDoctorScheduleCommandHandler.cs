using Clinio.Application.Common;
using Clinio.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clinio.Application.Features.Appointments.Commands.UpdateDoctorSchedule;

public class UpdateDoctorScheduleHandler(
    IApplicationDbContext db,
    ILocalizationService _localization,
    ITimeSlotGenerator _slotGenerator
) : IRequestHandler<UpdateDoctorScheduleCommand, Result<bool>>
{
    private const int DefaultDaysAhead = 30;

    public async Task<Result<bool>> Handle(UpdateDoctorScheduleCommand request, CancellationToken ct)
    {
        var schedule = await db.DoctorSchedules
            .FirstOrDefaultAsync(s => s.Id == request.ScheduleId, ct);

        if (schedule is null)
            return Result<bool>.Failure(new ResultError("ScheduleErrors.NotFound", _localization.Get("ScheduleErrors.NotFound")));

        schedule.StartTime = request.StartTime;
        schedule.EndTime = request.EndTime;
        schedule.SlotDurationMinutes = request.SlotDurationMinutes;

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        // delete future unbooked slots for this schedule
        var unbookedFutureSlots = await db.TimeSlots
            .Where(t => t.DoctorScheduleId == schedule.Id && t.Date >= today && !t.IsBooked)
            .ToListAsync(ct);

        db.TimeSlots.RemoveRange(unbookedFutureSlots);

        // keep booked slots as exclusion set so we don't duplicate them
        var bookedSlots = await db.TimeSlots
            .Where(t => t.DoctorScheduleId == schedule.Id && t.Date >= today && t.IsBooked)
            .Select(t => new { t.Date, t.StartTime })
            .ToListAsync(ct);

        var existingSet = bookedSlots.Select(s => (s.Date, s.StartTime)).ToHashSet();

        await db.SaveChangesAsync(ct);

        var newSlots = _slotGenerator.GenerateSlots(schedule, DefaultDaysAhead, existingSet);

        db.TimeSlots.AddRange(newSlots);
        await db.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }
}