namespace Clinio.Application.DTOs.Auth;

public record RegisterDto(
    string Username, 
    string Email, 
    string Password, 
    string FullName, 
    string Role
    );