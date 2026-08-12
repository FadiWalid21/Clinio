using Clinio.Domain.Entities.Clinics;

namespace Clinio.Domain.Entities.Appointments;

public class TimeSlot
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public bool IsBooked { get; set; } = false;

    public int DoctorId { get; set; }
    public int ClinicId { get; set; }
    public int DoctorScheduleId { get; set; }

    public Doctor Doctor { get; set; } = null!;
    public Clinic Clinic { get; set; } = null!;
    public DoctorSchedule DoctorSchedule { get; set; } = null!;
    public Appointment? Appointment { get; set; }
}