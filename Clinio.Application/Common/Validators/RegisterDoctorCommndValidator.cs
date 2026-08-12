using Clinio.Application.Features.Doctors.Commands.RegisterDoctor;
using Clinio.Application.Interfaces;
using FluentValidation;

namespace Clinio.Application.Common.Validators.Auth;

public class RegisterDoctorCommandValidator : AbstractValidator<RegisterDoctorCommand>
{
    public RegisterDoctorCommandValidator(ILocalizationService localization)
    {
        RuleFor(x => x.Email)
            .NotEmpty()
                .WithMessage(_ => localization.Get("Validation.Required"))
            .EmailAddress()
                .WithMessage(_ => localization.Get("Validation.InvalidEmail"));

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

        RuleFor(x => x.Password)
            .NotEmpty()
                .WithMessage(_ => localization.Get("Validation.Required"))
            .MinimumLength(8)
                .WithMessage(_ => localization.Get("Validation.MinLength", 8));

        RuleFor(x => x.Specialty)
            .NotEmpty()
                .WithMessage(_ => localization.Get("Validation.Required"))
            .MaximumLength(100)
                .WithMessage(_ => localization.Get("Validation.MaxLength", 100));

        RuleFor(x => x.LicenseNumber)
            .NotEmpty()
                .WithMessage(_ => localization.Get("Validation.Required"))
            .MaximumLength(50)
                .WithMessage(_ => localization.Get("Validation.MaxLength", 50));

        RuleFor(x => x.ConsultationFee)
            .GreaterThan(0)
                .WithMessage(_ => localization.Get("Validation.InvalidConsultationFee"));

        // Clinic — either join existing or register new, not both, not neither
        RuleFor(x => x)
            .Must(x => x.ClinicId.HasValue || x.RegisterClinic != null)
                .WithMessage(_ => localization.Get("Validation.ClinicRequired"));

        RuleFor(x => x)
            .Must(x => !(x.ClinicId.HasValue && x.RegisterClinic != null))
                .WithMessage(_ => localization.Get("Validation.ClinicConflict"));

        // If registering a new clinic, validate its fields
        When(x => x.RegisterClinic != null, () =>
        {
            RuleFor(x => x.RegisterClinic.Name)
                .NotEmpty()
                    .WithMessage(_ => localization.Get("Validation.Required"))
                .MaximumLength(100)
                    .WithMessage(_ => localization.Get("Validation.MaxLength", 100));

            RuleFor(x => x.RegisterClinic.Address)
                .NotEmpty()
                    .WithMessage(_ => localization.Get("Validation.Required"));

            RuleFor(x => x.RegisterClinic.Phone)
                .NotEmpty()
                    .WithMessage(_ => localization.Get("Validation.Required"))
                .Matches(@"^01[0125][0-9]{8}$")
                    .WithMessage(_ => localization.Get("Validation.InvalidPhoneNumber"));
        });
    }
}