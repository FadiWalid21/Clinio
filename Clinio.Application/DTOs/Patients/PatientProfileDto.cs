using System;

namespace Clinio.Application.DTOs.Patients;

public record PatientProfileDto(
    int Id,
    string FirstName,
    string LastName,
    string Email,
    string? PhoneNumber,
    string? Image,
    int Age,
    DateTime DateOfBirth,
    string Gender,
    string? BloodType,
    string? ChronicDiseases,
    string? Allergies
);