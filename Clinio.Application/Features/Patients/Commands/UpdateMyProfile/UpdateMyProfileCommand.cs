using Clinio.Application.Common;
using MediatR;

namespace Clinio.Application.Features.Patients.Commands.UpdateMyProfile;

public record UpdateMyProfileCommand(
    string FirstName,
    string LastName,
    string? PhoneNumber,
    DateTime DateOfBirth,
    string Gender,
    string? BloodType,
    string? ChronicDiseases,
    string? Allergies
) : IRequest<Result<bool>>;