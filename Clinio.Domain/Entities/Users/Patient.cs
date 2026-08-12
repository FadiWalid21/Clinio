using Clinio.Domain.Entities.Appointments;
using Clinio.Domain.Entities.Users;

namespace Clinio.Domain.Entities.Users;

public class Patient
{
    public int Id { get; set; }
    public int Age
    {
        get
        {
            var today = DateTime.Today;
            var age = today.Year - DateOfBirth.Year;

            if (DateOfBirth.Date > today.AddYears(-age))
                age--;

            return age;
        }
    }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; } = string.Empty;
    public string? BloodType { get; set; }
    public string? ChronicDiseases { get; set; }
    public string? Allergies { get; set; }
    
    public int UserId { get; set; }
    public ApplicationUser ApplicationUser { get; set; } = null!;

    // new
    public ICollection<Appointment> Appointments { get; set; } = [];
}