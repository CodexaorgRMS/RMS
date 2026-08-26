using FluentValidation;

namespace Finance.Application.Features.Shifts.Commands.CloseShift;

public sealed class CloseShiftCommandValidator : AbstractValidator<CloseShiftCommand>
{
    public CloseShiftCommandValidator()
    {
        RuleFor(x => x.ShiftId)
            .NotEmpty()
            .WithMessage("Shift ID is required.");

        RuleFor(x => x.ActualCash)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Actual cash must be greater than or equal to 0.");

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .WithMessage("Notes must not exceed 500 characters.")
            .When(x => x.Notes != null);
    }
}
