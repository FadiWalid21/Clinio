using Clinio.Application.Common;
using MediatR;

namespace Clinio.Application.Features.Auth.Commands.DeleteUserImage;

public record DeleteUserImageCommand(int UserId) : IRequest<Result<bool>>;
