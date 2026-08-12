using System;
using Clinio.Application.Features.Patients.Commands.UpdateMyProfile;
using FluentValidation;

namespace Clinio.Application.Common.Validators.Auth;

public class UpdateMyProfileValidator : AbstractValidator<UpdateMyProfileCommand>
{
    public UpdateMyProfileValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.PhoneNumber)
            .MaximumLength(20)
            .When(x => x.PhoneNumber is not null);

        RuleFor(x => x.DateOfBirth)
            .LessThan(DateTime.Today)
            .WithMessage("Date of birth must be in the past.");

        RuleFor(x => x.Gender)
            .NotEmpty()
            .Must(g => g is "Male" or "Female")
            .WithMessage("Gender must be Male or Female.");

        RuleFor(x => x.BloodType)
            .MaximumLength(5)
            .When(x => x.BloodType is not null);

        RuleFor(x => x.ChronicDiseases)
            .MaximumLength(500)
            .When(x => x.ChronicDiseases is not null);

        RuleFor(x => x.Allergies)
            .MaximumLength(500)
            .When(x => x.Allergies is not null);
    }
}