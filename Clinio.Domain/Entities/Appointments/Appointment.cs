using Clinio.Domain.Entities.Clinics;
using Clinio.Domain.Entities.Users;
using Clinio.Domain.Enums;

namespace Clinio.Domain.Entities.Appointments;

public class Appointment
{
    public int Id { get; set; }
    public AppointmentStatus Status { get; set; } = AppointmentStatus.Pending;
    public string? Notes { get; set; }
    public string? CancellationReason { get; set; }
    public DateTime? CancelledAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int PatientId { get; set; }
    public int DoctorId { get; set; }
    public int ClinicId { get; set; }
    public int TimeSlotId { get; set; }
    public int BookedById { get; set; }
    public int? CancelledById { get; set; }

    public Patient Patient { get; set; } = null!;
    public Doctor Doctor { get; set; } = null!;
    public Clinic Clinic { get; set; } = null!;
    public TimeSlot TimeSlot { get; set; } = null!;
    public ApplicationUser BookedBy { get; set; } = null!;
    public ApplicationUser? CancelledBy { get; set; }
}