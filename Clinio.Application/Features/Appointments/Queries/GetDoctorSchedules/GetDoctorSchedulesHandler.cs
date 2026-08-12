using Clinio.Application.Common;
using Clinio.Application.DTOs.Doctors;
using Clinio.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clinio.Application.Features.Appointments.Queries.GetDoctorSchedules;

public class GetDoctorSchedulesHandler(
    IApplicationDbContext db,
    ICurrentUserService _userService,
    ILocalizationService _localization
) : IRequestHandler<GetDoctorSchedulesQuery, Result<List<DoctorScheduleDto>>>
{
    public async Task<Result<List<DoctorScheduleDto>>> Handle(GetDoctorSchedulesQuery request, CancellationToken ct)
    {
        var userId = _userService.UserId;

        var doctor = await db.Doctors
            .FirstOrDefaultAsync(d => d.UserId == userId, ct);

        if (doctor is null)
            return Result<List<DoctorScheduleDto>>.Failure(
                new ResultError("Identity.UserNotFound", _localization.Get("Identity.UserNotFound")));

        var schedules = await db.DoctorSchedules
            .Where(s => s.DoctorId == doctor.Id)
            .OrderBy(s => s.ClinicId)
            .ThenBy(s => s.DayOfWeek)
            .Select(s => new DoctorScheduleDto(
                s.Id,
                s.ClinicId,
                s.Clinic.Name,
                s.DayOfWeek,
                s.StartTime,
                s.EndTime,
                s.SlotDurationMinutes,
                s.IsActive
            ))
            .ToListAsync(ct);

        return Result<List<DoctorScheduleDto>>.Success(schedules);
    }
}