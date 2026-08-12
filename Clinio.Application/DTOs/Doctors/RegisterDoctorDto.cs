using Clinio.Application.DTOs.Clinics;

namespace Clinio.Application.DTOs.Doctors;

public record RegisterDoctorDto(
    string Email,
    string FirstName,
    string LastName,
    string Password,
    // Doctor Profile
    string Specialty,
    string LicenseNumber,
    decimal ConsultationFee,
    RegisterClinicDto? RegisterClinic,
    int? ClinicId
    );