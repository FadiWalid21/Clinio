using Clinio.Application.Common;
using Clinio.Application.DTOs.Appointments;
using Clinio.Domain.Enums;
using MediatR;

namespace Clinio.Application.Features.Appointments.Queries.GetMyAppointments;

public record GetMyAppointmentsQuery(
    AppointmentStatus? Status = null
) : IRequest<Result<List<MyAppointmentDto>>>;