namespace Clinio.Application.DTOs.Doctors;

public record DoctorDto(
    int Id,
    string FullName,
    int ClinicId,
    string ClinicName,
    string Specialty,
    string? ProfileImageUrl
);