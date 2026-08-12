using Clinio.Application.Common;
using Clinio.Application.Interfaces;
using Clinio.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clinio.Application.Features.Appointments.Commands.ConfirmAppointment;
public class ConfirmAppointmentHandler(
    IApplicationDbContext db,
    ICurrentUserService _userService,
    ILocalizationService _localization
) : IRequestHandler<ConfirmAppointmentCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(ConfirmAppointmentCommand request, CancellationToken ct)
    {
        var userId = _userService.UserId;

        var appointment = await db.Appointments
            .Include(a => a.Doctor)
            .Include(a => a.TimeSlot)
            .FirstOrDefaultAsync(a => a.Id == request.AppointmentId, ct);

        if (appointment is null)
            return Result<bool>.Failure(
                new ResultError("Appointment.NotFound", _localization.Get("Appointment.NotFound")));

        // only the doctor of this appointment or a secretary in the same clinic can confirm
        var doctor = await db.Doctors
            .FirstOrDefaultAsync(d => d.UserId == userId, ct);

        var secretary = doctor is null
            ? await db.Secretaries
                .FirstOrDefaultAsync(s => s.UserId == userId && s.ClinicId == appointment.ClinicId, ct)
            : null;

        if (doctor?.Id != appointment.DoctorId && secretary is null)
            return Result<bool>.Failure(
                new ResultError("Identity.Forbidden", _localization.Get("Identity.Forbidden")));

        if (appointment.Status != AppointmentStatus.Pending)
            return Result<bool>.Failure(
                new ResultError("Appointment.InvalidStatus", _localization.Get("Appointment.InvalidStatus")));

        appointment.Status = AppointmentStatus.Confirmed;

        await db.SaveChangesAsync(ct);

        return Result<bool>.Success(true);
    }
}