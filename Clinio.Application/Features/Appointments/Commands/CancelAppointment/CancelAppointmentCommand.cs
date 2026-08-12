using Clinio.Application.Common;
using MediatR;

namespace Clinio.Application.Features.Appointments.Commands.CancelAppointment;

public record CancelAppointmentCommand(
    int AppointmentId,
    string? CancellationReason
) : IRequest<Result<bool>>;