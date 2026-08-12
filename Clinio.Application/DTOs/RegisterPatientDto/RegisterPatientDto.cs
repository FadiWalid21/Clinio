using System;

namespace Clinio.Application.DTOs.RegisterPatientDto;

public record RegisterPatientDto(
    string Email,
    string Password,
    string FirstName,
    string LastName,
    DateTime DateOfBirth,
    string Gender,
    string? BloodType,
    string? ChronicDiseases,
    string? Allergies
    );