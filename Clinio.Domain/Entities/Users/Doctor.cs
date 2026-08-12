using Clinio.Domain.Entities.Appointments;
using Clinio.Domain.Entities.Clinics;
using Clinio.Domain.Entities.Users;

public class Doctor
{
    public int Id { get; set; }
    public string Specialty { get; set; } = string.Empty;
    public string LicenseNumber { get; set; } = string.Empty;
    public decimal ConsultationFee { get; set; }
    
    public int UserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; } = null!;
    
    public int ClinicId { get; set; }
    public Clinic Clinic { get; set; } = null!;
    public ICollection<DoctorSchedule> Schedules { get; set; } = [];
    public ICollection<TimeSlot> TimeSlots { get; set; } = [];
    public ICollection<Appointment> Appointments { get; set; } = [];
}