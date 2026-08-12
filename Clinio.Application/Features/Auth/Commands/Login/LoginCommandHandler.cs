using Clinio.Application.Common;
using Clinio.Application.DTOs.Auth;
using Clinio.Application.Interfaces;
using Clinio.Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Clinio.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler(
    UserManager<ApplicationUser> _userManager,
    IJwtProvider _jwtProvider,
    ILocalizationService _localization)
    : IRequestHandler<LoginCommand, Result<AuthResponseDto>>
{
    public async Task<Result<AuthResponseDto>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user is null)
            return Result<AuthResponseDto>.Failure(new ResultError("Auth.InvalidCredentials",_localization.Get("Auth.InvalidCredentials")));

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Password);

        if (!isPasswordValid)
            return Result<AuthResponseDto>.Failure(new ResultError("Auth.InvalidCredentials",_localization.Get("Auth.InvalidCredentials")));

        var roles = await _userManager.GetRolesAsync(user);
        var accessToken = _jwtProvider.GenerateToken(user, roles);
        var refreshToken = _jwtProvider.GenerateRefreshToken();

        user.RefreshTokens ??= new List<RefreshToken>();
        user.RefreshTokens.RemoveAll(t => t.ExpiresOn <= DateTime.UtcNow);
        user.RefreshTokens.Add(refreshToken);

        await _userManager.UpdateAsync(user);

        var response = new AuthResponseDto(
            IsAuthenticated: true,
            Message: _localization.Get("Auth.LoginSuccess"),
            Username: user.UserName!,
            Email: user.Email!,
            Token: accessToken,
            RefreshToken: refreshToken.Token,
            RefreshTokenExpiration: refreshToken.ExpiresOn
        );

        return Result<AuthResponseDto>.Success(response);
    }
}