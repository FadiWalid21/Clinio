using Clinio.Application.Common;
using Clinio.Application.Interfaces;
using Clinio.Domain.Entities.Appointments;
using Clinio.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Clinio.Application.Features.Appointments.Commands.CreateAppointment;

public class CreateAppointmentHandler(
    IApplicationDbContext db,
    ICurrentUserService _userService,
    ILocalizationService _localization,
    IClinicClock _clock
) : IRequestHandler<CreateAppointmentCommand, Result<int>>
{
    public async Task<Result<int>> Handle(CreateAppointmentCommand request, CancellationToken ct)
    {
        var userId = _userService.UserId;

        var patient = await db.Patients
            .FirstOrDefaultAsync(p => p.UserId == userId, ct);

        if (patient is null)
            return Result<int>.Failure(
                new ResultError("Identity.UserNotFound", _localization.Get("Identity.UserNotFound")));

        await using var transaction = await db.Database.BeginTransactionAsync(ct);

        try
        {
            var slot = await db.TimeSlots
                .FirstOrDefaultAsync(t => t.Id == request.TimeSlotId, ct);

            if (slot is null)
                return Result<int>.Failure(
                    new ResultError("Appointment.SlotNotFound", _localization.Get("Appointment.SlotNotFound")));

            if (slot.IsBooked)
                return Result<int>.Failure(
                    new ResultError("Appointment.SlotAlreadyBooked", _localization.Get("Appointment.SlotAlreadyBooked")));

            if (slot.DoctorId != request.DoctorId || slot.ClinicId != request.ClinicId)
                return Result<int>.Failure(
                    new ResultError("Appointment.SlotMismatch", _localization.Get("Appointment.SlotMismatch")));

            var slotDateTime = slot.Date.ToDateTime(slot.StartTime);
            if (slotDateTime <= _clock.Now)   // ← بدل DateTime.UtcNow
                return Result<int>.Failure(
                    new ResultError("Appointment.SlotExpired", _localization.Get("Appointment.SlotExpired")));

            slot.IsBooked = true;

            var appointment = new Appointment
            {
                PatientId = patient.Id,
                DoctorId = request.DoctorId,
                ClinicId = request.ClinicId,
                TimeSlotId = request.TimeSlotId,
                BookedById = userId!.Value,
                Status = AppointmentStatus.Pending,
                Notes = request.Notes,
                CreatedAt = DateTime.UtcNow   // ← ده سيبه UtcNow، صح كده لأنه بس timestamp تسجيل مش مقارنة
            };

            db.Appointments.Add(appointment);
            await db.SaveChangesAsync(ct);
            await transaction.CommitAsync(ct);

            return Result<int>.Success(appointment.Id);
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            return Result<int>.Failure(
                new ResultError("Appointment.BookingFailed", _localization.Get("Appointment.BookingFailed")));
        }
    }
}