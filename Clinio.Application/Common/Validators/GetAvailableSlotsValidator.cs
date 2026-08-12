using Clinio.Application.Features.Appointments.Queries.GetAvailableSlots;
using FluentValidation;

namespace Clinio.Application.Common.Validators.Auth;

public class GetAvailableSlotsValidator : AbstractValidator<GetAvailableSlotsQuery>
{
    public GetAvailableSlotsValidator()
    {
        RuleFor(x => x.ToDate)
            .GreaterThanOrEqualTo(x => x.FromDate)
            .WithMessage("ToDate must be after or equal to FromDate.");

        RuleFor(x => x)
            .Must(x => (x.ToDate.DayNumber - x.FromDate.DayNumber) <= 60)
            .WithMessage("Date range cannot exceed 60 days.");
    }
}