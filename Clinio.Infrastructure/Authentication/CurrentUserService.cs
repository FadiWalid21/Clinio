using System.Security.Claims;
using Clinio.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Clinio.Infrastructure.Authentication;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public int? UserId
    {
        get
        {
            var userId = _httpContextAccessor.HttpContext?.User
                .FindFirstValue(ClaimTypes.NameIdentifier);

            return int.TryParse(userId, out var id)
                ? id
                : null;
        }
    }

    public string? Email =>
        _httpContextAccessor.HttpContext?.User
            .FindFirstValue(ClaimTypes.Email);

}