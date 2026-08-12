using System;

namespace Clinio.Application.DTOs.Auth;

public record AuthResponseDto(
    bool IsAuthenticated, 
    string Message, 
    string Username, 
    string Email, 
    string Token, 
    string RefreshToken, 
    DateTime RefreshTokenExpiration
    );