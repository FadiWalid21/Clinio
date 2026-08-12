using Microsoft.AspNetCore.Identity;

namespace Clinio.Domain.Entities.Users;

public class ApplicationRole : IdentityRole<int>
{
    public ApplicationRole() : base()
    {
    }

    public ApplicationRole(string roleName) : base(roleName)
    {
    }
}