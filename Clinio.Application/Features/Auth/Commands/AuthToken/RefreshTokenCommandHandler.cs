using System.Security.Claims;
using Clinio.Application.Common;
using Clinio.Application.DTOs.Auth;
using Clinio.Application.Interfaces;
using Clinio.Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Clinio.Application.Features.Auth.Commands.AuthToken;

public class RefreshTokenCommandHandler(IJwtProvider _jwtProvider, UserManager<ApplicationUser> _userManager , ILocalizationService _localization)
    : IRequestHandler<RefreshTokenCommand, Result<AuthResponseDto>>
{
    public async Task<Result<AuthResponseDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var principal = _jwtProvider.GetPrincipalFromExpiredToken(request.Token);

        if (principal is null)
            return Result<AuthResponseDto>.Failure(new ResultError("Token.Invalid", _localization.Get("Auth.Token.InvalidToken")));

        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier);

        if (userIdClaim is null)
            return Result<AuthResponseDto>.Failure(new ResultError("Token.Invalid", _localization.Get("Auth.Token.InvalidToken")));

        if (!int.TryParse(userIdClaim.Value, out var userId))
            return Result<AuthResponseDto>.Failure(new ResultError("Token.Invalid", _localization.Get("Auth.Token.InvalidToken")));

        var user = await _userManager.Users
            .Include(x => x.RefreshTokens)
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        if (user is null)
            return Result<AuthResponseDto>.Failure(new ResultError("User.NotFound", _localization.Get("Auth.Token.UserNotFound")));

        var refreshToken = user.RefreshTokens
            .FirstOrDefault(x => x.Token == request.RefreshToken);

        if (refreshToken is null)
            return Result<AuthResponseDto>.Failure(new ResultError("Token.RefreshInvalid", _localization.Get("Auth.Token.RefreshInvalid")));

        if (!refreshToken.IsActive)
            return Result<AuthResponseDto>.Failure(new ResultError("Token.RefreshExpired", _localization.Get("Auth.Token.RefreshExpired")));

        var roles = await _userManager.GetRolesAsync(user);
        var newAccessToken = _jwtProvider.GenerateToken(user, roles);
        var newRefreshToken = _jwtProvider.GenerateRefreshToken();

        refreshToken.RevokedOn = DateTime.UtcNow;
        user.RefreshTokens.Add(newRefreshToken);

        var updateResult = await _userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
            return Result<AuthResponseDto>.Failure(new ResultError("Token.UpdateFailed", _localization.Get("Auth.Token.RefreshInvalid")));

        var response = new AuthResponseDto(
            true, 
            _localization.Get("Auth.Token.RefreshSuccess"),
            user.UserName ?? string.Empty,
            user.Email ?? string.Empty,
            newAccessToken,
            newRefreshToken.Token,
            newRefreshToken.ExpiresOn
        );

        return Result<AuthResponseDto>.Success(response);
    }
}