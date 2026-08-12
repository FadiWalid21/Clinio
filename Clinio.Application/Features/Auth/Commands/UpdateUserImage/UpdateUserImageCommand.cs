using Clinio.Application.Common;
using MediatR;

namespace Clinio.Application.Features.Auth.Commands.UpdateUserImage;

public record UpdateUserImageCommand(int UserId, ImageUploadRequest Image) : IRequest<Result<string>>;
