using Clinio.Application.Features.Patients.Commands.RegisterPatient;
using Clinio.Application.Interfaces;
using FluentValidation;

namespace Clinio.Application.Common.Validators.Auth;

public class RegisterPatientCommandValidator : AbstractValidator<RegisterPatientCommand>
{
    public RegisterPatientCommandValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(_ => localization.Get("Validation.Required"))
            .EmailAddress()
            .WithMessage(_ => localization.Get("Validation.InvalidEmail"));

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(_ => localization.Get("Validation.Required"))
            .MinimumLength(8)
            .WithMessage(_ => localization.Get("Validation.MinLength", 8));

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .WithMessage(_ => localization.Get("Validation.Required"))
            .MaximumLength(50)
            .WithMessage(_ => localization.Get("Validation.MaxLength", 50));

        RuleFor(x => x.LastName)
            .NotEmpty()
            .WithMessage(_ => localization.Get("Validation.Required"))
            .MaximumLength(50)
            .WithMessage(_ => localization.Get("Validation.MaxLength", 50));

        RuleFor(x => x.DateOfBirth)
            .NotEmpty()
            .WithMessage(_ => localization.Get("Validation.Required"))
            .LessThan(DateTime.UtcNow)
            .WithMessage(_ => localization.Get("Validation.InvalidDateOfBirth"));

        RuleFor(x => x.Gender)
            .NotEmpty()
            .WithMessage(_ => localization.Get("Validation.Required"))
            .Must(g => g is "Male" or "Female")
            .WithMessage(_ => localization.Get("Validation.InvalidGender"));
    }
}