using Clinio.Application.Common;
using Clinio.Application.Common.Errors;
using Clinio.Application.Features.Auth.Commands.UpdateUserImage;
using Clinio.Application.Interfaces;
using Clinio.Domain.Entities.Users;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Clinio.Application.Features.Auth.Commands.UpdateUserImage;

public class UpdateUserImageHandler(
    IImageService _imageService,
    IApplicationDbContext _context,
    UserManager<ApplicationUser> _userManager) : IRequestHandler<UpdateUserImageCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UpdateUserImageCommand request, CancellationToken ct)
    {
        var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == request.UserId, ct);
        
        if (user is null)
            return Result<string>.Failure(new ResultError("Auth.UserNotFound",  "Auth.UserNotFound"));

        if (user.ImageFileName is not null)
            await _imageService.DeleteImageAsync(user.ImageFileName);

        var uploadResult = await _imageService.UploadUserImageAsync(request.Image, request.UserId);

        if (!uploadResult.IsSuccess)
            return uploadResult;

        user.Image = uploadResult.Value;
        user.ImageFileName = uploadResult.Value;

        await _context.SaveChangesAsync(ct);

        return Result<string>.Success(user.Image!);
    }
}