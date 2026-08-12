namespace Clinio.Application.Interfaces;

public interface ICurrentUserService
{
    int?  UserId { get; }
    string? Email { get; }
}