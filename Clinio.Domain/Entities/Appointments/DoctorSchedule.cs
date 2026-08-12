using Clinio.Domain.Entities.Clinics;

namespace Clinio.Domain.Entities.Appointments;

public class DoctorSchedule
{
    public int Id { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public int SlotDurationMinutes { get; set; }
    public bool IsActive { get; set; } = true;

    public int DoctorId { get; set; }
    public int ClinicId { get; set; }

    public Doctor Doctor { get; set; } = null!;
    public Clinic Clinic { get; set; } = null!;
    public ICollection<TimeSlot> TimeSlots { get; set; } = [];
}