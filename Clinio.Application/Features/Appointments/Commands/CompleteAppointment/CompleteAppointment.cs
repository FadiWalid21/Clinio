using Clinio.Application.Common;
using MediatR;

namespace Clinio.Application.Features.Appointments.Commands.CompleteAppointment;

public record CompleteAppointmentCommand(int AppointmentId) : IRequest<Result<bool>>;
