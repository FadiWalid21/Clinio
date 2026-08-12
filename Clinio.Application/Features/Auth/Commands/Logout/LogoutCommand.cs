using Clinio.Application.Common;
using MediatR;

namespace Clinio.Application.Features.Auth.Commands.Logout;

public record LogoutCommand(string? RefreshToken , bool LogoutFromAllDevices = false) : IRequest<Result<bool>>;