using Clinio.Application.Common;
using Clinio.Application.DTOs.Doctors;
using MediatR;

namespace Clinio.Application.Features.Appointments.Queries.GetDoctorSchedules;

public record GetDoctorSchedulesQuery() : IRequest<Result<List<DoctorScheduleDto>>>;
