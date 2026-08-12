using Clinio.Application.Common;
using Clinio.Application.DTOs.Auth;
using Clinio.Application.DTOs.Clinics;
using MediatR;

namespace Clinio.Application.Features.Doctors.Commands.RegisterDoctor;

public record RegisterDoctorCommand(
    // User Acc Info
    string Email,
    string FirstName,
    string LastName,
    string Password,
    // Doctor Profile
    string Specialty,
    string LicenseNumber,
    decimal ConsultationFee,
    RegisterClinicDto? RegisterClinic,
    int? ClinicId,
    ImageUploadRequest? Image = null
    )  : IRequest<Result<AuthResponseDto>>;