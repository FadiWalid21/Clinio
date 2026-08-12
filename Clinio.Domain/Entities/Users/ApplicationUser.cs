using Microsoft.AspNetCore.Identity;
namespace Clinio.Domain.Entities.Users;

public class ApplicationUser : IdentityUser<int>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; }  = string.Empty;
    // public string UserType { get; set; }
    public string? Image { get; set; }
    public string? ImageFileName { get; set; } 
    
    public Doctor? DoctorProfile { get; set; }
    public Patient? PatientProfile { get; set; }
    public Secretary? SecretaryProfile { get; set; }
    public List<RefreshToken> RefreshTokens { get; set; } = new();
}