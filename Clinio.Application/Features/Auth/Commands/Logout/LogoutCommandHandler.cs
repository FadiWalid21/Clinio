using Clinio.Application.Common;
using Clinio.Application.Interfaces;
using Clinio.Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Clinio.Application.Features.Auth.Commands.Logout;

public class LogoutCommandHandler(UserManager<ApplicationUser> _userManager, ICurrentUserService _userService , ILocalizationService _localization) : IRequestHandler<LogoutCommand , Result<bool>>
{
    public async Task<Result<bool>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        if (_userService.UserId is null)
            return Result<bool>.Failure(new ResultError("Unauthorized", _localization.Get("Auth.Unauthorized")));

        var user = await _userManager.FindByIdAsync(
            _userService.UserId.Value.ToString());

        if (user is null)
            return Result<bool>.Failure(new ResultError( "User not found", _localization.Get("Auth.UserNotFound")));

        if (request.LogoutFromAllDevices)
        {
            foreach (var token in user.RefreshTokens.Where(x => x.IsActive))
            {
                token.RevokedOn = DateTime.UtcNow;
            }
        }
        else
        {
            if (string.IsNullOrWhiteSpace(request.RefreshToken))
                return Result<bool>.Failure(new ResultError( "Refresh token is required", _localization.Get("Auth.Unauthorized")));

            var refreshToken = user.RefreshTokens
                .SingleOrDefault(x => x.Token == request.RefreshToken);

            if (refreshToken is null)
                return Result<bool>.Failure(new ResultError("Refresh token not found" ,  _localization.Get("Auth.Unauthorized")));

            refreshToken.RevokedOn = DateTime.UtcNow;
        }

        var updateResult = await _userManager.UpdateAsync(user);

        if (!updateResult.Succeeded)
            return Result<bool>.Failure(new ResultError("Failed to logout user" , _localization.Get("Auth.LogoutFailed")));

        return Result<bool>.Success(true);
    }
}