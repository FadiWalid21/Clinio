using Clinio.Application.Common;
using Clinio.Application.DTOs.Auth;
using MediatR;

namespace Clinio.Application.Features.Auth.Commands.AuthToken;

public record RefreshTokenCommand(
    string Token,
    string RefreshToken
    ) : IRequest<Result<AuthResponseDto>>;