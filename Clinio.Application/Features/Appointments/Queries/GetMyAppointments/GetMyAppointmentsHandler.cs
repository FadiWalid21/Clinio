using Clinio.Application.Common;
using Clinio.Application.DTOs.Appointments;
using Clinio.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clinio.Application.Features.Appointments.Queries.GetMyAppointments;

public class GetMyAppointmentsHandler(
    IApplicationDbContext db,
    ICurrentUserService _userService,
    ILocalizationService _localization
) : IRequestHandler<GetMyAppointmentsQuery, Result<List<MyAppointmentDto>>>
{
    public async Task<Result<List<MyAppointmentDto>>> Handle(GetMyAppointmentsQuery request, CancellationToken ct)
    {
        var userId = _userService.UserId;

        var patient = await db.Patients
            .FirstOrDefaultAsync(p => p.UserId == userId, ct);

        if (patient is null)
            return Result<List<MyAppointmentDto>>.Failure(
                new ResultError("Identity.UserNotFound", _localization.Get("Identity.UserNotFound")));

        var query = db.Appointments
            .Where(a => a.PatientId == patient.Id);

        if (request.Status.HasValue)
            query = query.Where(a => a.Status == request.Status.Value);

        var appointments = await query
            .OrderByDescending(a => a.TimeSlot.Date)
            .ThenByDescending(a => a.TimeSlot.StartTime)
            .Select(a => new MyAppointmentDto(
                a.Id,
                a.TimeSlot.Date,
                a.TimeSlot.StartTime,
                a.TimeSlot.EndTime,
                a.Doctor.ApplicationUser.FirstName + " " + a.Doctor.ApplicationUser.LastName,
                a.Clinic.Name,
                a.Clinic.Address,
                a.Doctor.ConsultationFee,
                a.Status,
                a.Notes,
                a.CancellationReason,
                a.CreatedAt
            ))
            .ToListAsync(ct);

        return Result<List<MyAppointmentDto>>.Success(appointments);
    }
}