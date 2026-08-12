using Clinio.Application.Features.Appointments.Commands.CreateAppointment;
using FluentValidation;

namespace Clinio.Application.Common.Validators.Auth;

public class CreateAppointmentValidator : AbstractValidator<CreateAppointmentCommand>
{
    public CreateAppointmentValidator()
    {
        RuleFor(x => x.TimeSlotId)
            .GreaterThan(0);

        RuleFor(x => x.DoctorId)
            .GreaterThan(0);

        RuleFor(x => x.ClinicId)
            .GreaterThan(0);

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .When(x => x.Notes is not null);
    }
}