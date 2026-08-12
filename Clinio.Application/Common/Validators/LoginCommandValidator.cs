using Clinio.Application.Features.Auth.Commands.Login;
using Clinio.Application.Interfaces;
using FluentValidation;

namespace Clinio.Application.Common.Validators.Auth;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(_ => localization.Get("Validation.Required"))
            .EmailAddress()
            .WithMessage(_ => localization.Get("Validation.InvalidEmail"));

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(_ => localization.Get("Validation.Required"));
    }
}