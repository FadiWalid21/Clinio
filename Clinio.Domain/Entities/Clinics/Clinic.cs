using Clinio.Domain.Entities.Appointments;
using Clinio.Domain.Entities.Users;
namespace Clinio.Domain.Entities.Clinics;

public class Clinic
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public ICollection<ClinicImage> Images { get; set; } = [];
    
    public ICollection<Doctor> Doctors { get; set; } = new List<Doctor>();
    public ICollection<Secretary> Secretaries { get; set; } = new List<Secretary>();
    public ICollection<DoctorSchedule> Schedules { get; set; } = [];
    public ICollection<TimeSlot> TimeSlots { get; set; } = [];
    public ICollection<Appointment> Appointments { get; set; } = [];
}