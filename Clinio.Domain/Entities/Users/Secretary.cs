using Clinio.Domain.Entities.Clinics;

namespace Clinio.Domain.Entities.Users;

public class Secretary
{
    public int Id { get; set; }
    
    public int UserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; } = null!;
    public int ClinicId { get; set; }
    public Clinic Clinic { get; set; } = null!;
}