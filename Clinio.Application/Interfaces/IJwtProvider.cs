using System.Security.Claims;
using Clinio.Domain.Entities.Users;

namespace Clinio.Application.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(ApplicationUser user, IList<string> roles);
    
    RefreshToken GenerateRefreshToken();
    ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
}