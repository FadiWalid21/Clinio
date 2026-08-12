namespace Clinio.Application.DTOs.Doctors;

public record DoctorScheduleDto(
    int Id,
    int ClinicId,
    string ClinicName,
    DayOfWeek DayOfWeek,
    TimeOnly StartTime,
    TimeOnly EndTime,
    int SlotDurationMinutes,
    bool IsActive
);