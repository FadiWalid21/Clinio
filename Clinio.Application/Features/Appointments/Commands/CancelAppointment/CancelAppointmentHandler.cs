using Clinio.Application.Common;
using Clinio.Application.Interfaces;
using Clinio.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clinio.Application.Features.Appointments.Commands.CancelAppointment;

public class CancelAppointmentHandler(
    IApplicationDbContext db,
    ICurrentUserService _userService,
    ILocalizationService _localization
) : IRequestHandler<CancelAppointmentCommand, Result<bool>>
{
    private const int CancellationWindowMinutes = 15;

    public async Task<Result<bool>> Handle(CancelAppointmentCommand request, CancellationToken ct)
    {
        var userId = _userService.UserId;

        var appointment = await db.Appointments
            .Include(a => a.TimeSlot)
            .FirstOrDefaultAsync(a => a.Id == request.AppointmentId, ct);

        if (appointment is null)
            return Result<bool>.Failure(
                new ResultError("Appointment.NotFound", _localization.Get("Appointment.NotFound")));

        // check caller is the patient who booked, or a secretary in the same clinic
        var patient = await db.Patients
            .FirstOrDefaultAsync(p => p.UserId == userId, ct);

        var secretary = patient is null
            ? await db.Secretaries
                .FirstOrDefaultAsync(s => s.UserId == userId && s.ClinicId == appointment.ClinicId, ct)
            : null;

        if (patient?.Id != appointment.PatientId && secretary is null)
            return Result<bool>.Failure(
                new ResultError("Identity.Forbidden", _localization.Get("Identity.Forbidden")));

        if (appointment.Status is AppointmentStatus.Completed or AppointmentStatus.Cancelled)
            return Result<bool>.Failure(
                new ResultError("Appointment.InvalidStatus", _localization.Get("Appointment.InvalidStatus")));

        // 15 min cancellation rule
        var slotDateTime = appointment.TimeSlot.Date.ToDateTime(appointment.TimeSlot.StartTime);
        var canCancelUntil = slotDateTime.AddMinutes(-CancellationWindowMinutes);

        if (DateTime.UtcNow > canCancelUntil)
            return Result<bool>.Failure(
                new ResultError("Appointment.CancellationWindowPassed", _localization.Get("Appointment.CancellationWindowPassed")));

        appointment.Status = AppointmentStatus.Cancelled;
        appointment.CancelledById = userId!.Value;
        appointment.CancellationReason = request.CancellationReason;
        appointment.CancelledAt = DateTime.UtcNow;

        // free the slot back
        appointment.TimeSlot.IsBooked = false;

        await db.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }
}