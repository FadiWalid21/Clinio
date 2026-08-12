using Clinio.Application.Common;
using Clinio.Application.DTOs.Doctors;
using MediatR;

namespace Clinio.Application.Features.Doctors.Queries.GetAllDoctors;

public record GetAllDoctorsQuery(string? SearchTerm) : IRequest<Result<List<DoctorDto>>>;