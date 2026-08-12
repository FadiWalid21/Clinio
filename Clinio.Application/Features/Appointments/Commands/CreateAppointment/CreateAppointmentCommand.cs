using Clinio.Application.Common;
using MediatR;

namespace Clinio.Application.Features.Appointments.Commands.CreateAppointment;

public record CreateAppointmentCommand(
    int TimeSlotId,
    int DoctorId,
    int ClinicId,
    string? Notes
) : IRequest<Result<int>>;