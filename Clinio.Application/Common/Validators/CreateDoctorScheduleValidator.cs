using Clinio.Application.Features.Appointments.Commands.CreateDoctorSchedule;
using Clinio.Application.Interfaces;
using FluentValidation;

namespace Clinio.Application.Common.Validators.Auth;

public class CreateDoctorScheduleValidator : AbstractValidator<CreateDoctorScheduleCommand>
{
    public CreateDoctorScheduleValidator(ILocalizationService localize)
    {
        RuleFor(x => x.SlotDurationMinutes)
            .GreaterThan(0)
            .LessThanOrEqualTo(120)
            .WithMessage(localize.Get("Validaion.Doctor.Slot"));

        RuleFor(x => x.StartTime)
            .LessThan(x => x.EndTime)
        .WithMessage(localize.Get("Validaion.Doctor.StartTime"));

        RuleFor(x => x.EndTime)
            .GreaterThan(x => x.StartTime)
            .WithMessage(localize.Get("Validaion.Doctor.EndTime"));

        RuleFor(x => x)
            .Must(x =>
            {
                var totalMinutes = (x.EndTime - x.StartTime).TotalMinutes;
                return totalMinutes >= x.SlotDurationMinutes;
            })
            .WithMessage("The time range must fit at least one slot.");
    }
}