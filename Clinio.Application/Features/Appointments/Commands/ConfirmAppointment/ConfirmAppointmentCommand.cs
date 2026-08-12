using Clinio.Application.Common;
using MediatR;

namespace Clinio.Application.Features.Appointments.Commands.ConfirmAppointment;

public record ConfirmAppointmentCommand(int AppointmentId) : IRequest<Result<bool>>;
