using Clinio.Application.Common;
using Clinio.Application.DTOs.Auth;
using MediatR;

namespace Clinio.Application.Features.Auth.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<Result<AuthResponseDto>>;