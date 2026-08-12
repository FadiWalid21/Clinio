using Clinio.Application.Common;
using Clinio.Application.DTOs.Appointments;
using MediatR;

namespace Clinio.Application.Features.Appointments.Queries.GetAvailableSlots;

public record GetAvailableSlotsQuery(
    int DoctorId,
    int ClinicId,
    DateOnly FromDate,
    DateOnly ToDate
) : IRequest<Result<List<AvailableSlotDto>>>;