using Clinio.Application.Common;
using Clinio.Application.DTOs.Patients;
using MediatR;

namespace Clinio.Application.Features.Patients.Queries.GetMyProfile;

public record GetMyProfileQuery : IRequest<Result<PatientProfileDto>>;
