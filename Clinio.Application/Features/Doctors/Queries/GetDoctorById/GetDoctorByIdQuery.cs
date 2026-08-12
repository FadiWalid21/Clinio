using Clinio.Application.Common;
using Clinio.Application.DTOs.Doctors;
using MediatR;

namespace Clinio.Application.Features.Doctors.Queries.GetDoctorById;

public record GetDoctorByIdQuery(int Id) : IRequest<Result<DoctorDto>>;