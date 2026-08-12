using Clinio.Application.Common;
using Clinio.Application.Interfaces;
using Clinio.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clinio.Application.Features.Appointments.Commands.CompleteAppointment;

public class CompleteAppointmentHandler(
    IApplicationDbContext db,
    ICurrentUserService _userService,
    ILocalizationService _localization
) : IRequestHandler<CompleteAppointmentCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(CompleteAppointmentCommand request, CancellationToken ct)
    {
        var userId = _userService.UserId;

        var appointment = await db.Appointments
            .Include(a => a.TimeSlot)
            .FirstOrDefaultAsync(a => a.Id == request.AppointmentId, ct);

        if (appointment is null)
            return Result<bool>.Failure(
                new ResultError("Appointment.NotFound", _localization.Get("Appointment.NotFound")));

        // only the doctor of this appointment can mark it complete
        var doctor = await db.Doctors
            .FirstOrDefaultAsync(d => d.UserId == userId, ct);

        if (doctor is null || doctor.Id != appointment.DoctorId)
            return Result<bool>.Failure(
                new ResultError("Identity.Forbidden", _localization.Get("Identity.Forbidden")));

        if (appointment.Status != AppointmentStatus.Confirmed)
            return Result<bool>.Failure(
                new ResultError("Appointment.InvalidStatus", _localization.Get("Appointment.InvalidStatus")));

        // slot datetime must have already passed
        var slotDateTime = appointment.TimeSlot.Date.ToDateTime(appointment.TimeSlot.StartTime);
        if (DateTime.UtcNow < slotDateTime)
            return Result<bool>.Failure(
                new ResultError("Appointment.NotYetOccurred", _localization.Get("Appointment.NotYetOccurred")));

        appointment.Status = AppointmentStatus.Completed;

        await db.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }
}