namespace Clinio.Application.DTOs.Clinics;

public record RegisterClinicDto(
    string Name,
    string Address,
    string Phone
    );