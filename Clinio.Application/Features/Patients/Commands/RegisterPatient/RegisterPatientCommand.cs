using Clinio.Application.Common;
using Clinio.Application.DTOs.Auth;
using MediatR;

namespace Clinio.Application.Features.Patients.Commands.RegisterPatient;

public record RegisterPatientCommand(
    // for acc
    string Email, 
    string Password, 
    string FirstName,
    string LastName,
    // patient acc
    DateTime DateOfBirth,
    string Gender,
    string? BloodType,
    string? ChronicDiseases,
    string? Allergies,
    // optional image
    ImageUploadRequest? Image = null
) : IRequest<Result<AuthResponseDto>>;