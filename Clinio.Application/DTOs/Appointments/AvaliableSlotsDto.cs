namespace Clinio.Application.DTOs.Appointments;

public record AvailableSlotDto(
    int Id,
    DateOnly Date,
    TimeOnly StartTime,
    TimeOnly EndTime
);