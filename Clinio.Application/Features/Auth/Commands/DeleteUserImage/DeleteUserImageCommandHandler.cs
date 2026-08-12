using Clinio.Application.Common;
using Clinio.Application.Common.Errors;
using Clinio.Application.Interfaces;
using Clinio.Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Clinio.Application.Features.Auth.Commands.DeleteUserImage;

public class DeleteUserImageCommandHandler(IImageService imageService,
    UserManager<ApplicationUser> userManager
) : IRequestHandler<DeleteUserImageCommand, Result<bool>>
{
    public async Task<Result<bool>> Handle(DeleteUserImageCommand request, CancellationToken ct)
    {
        var user = await userManager.Users
            .FirstOrDefaultAsync(u => u.Id == request.UserId, ct);

        if (user is null)
            return Result<bool>.Failure(new ResultError("Auth.UserNotFound" , "Auth.UserNotFound"));

        if (user.ImageFileName is null)
            return Result<bool>.Failure(new ResultError("Image.NotFound", "Image.NotFound"));

        var deleteResult = await imageService.DeleteImageAsync(user.ImageFileName);

        if (!deleteResult.IsSuccess)
            return Result<bool>.Failure(deleteResult.Error!);

        user.Image = null;
        user.ImageFileName = null;

        await userManager.UpdateAsync(user);

        return Result<bool>.Success(true);
    }
}