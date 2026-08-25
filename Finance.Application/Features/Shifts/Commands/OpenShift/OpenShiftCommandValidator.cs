using FluentValidation;

namespace Finance.Application.Features.Shifts.Commands.OpenShift;

public sealed class OpenShiftCommandValidator : AbstractValidator<OpenShiftCommand>
{
    public OpenShiftCommandValidator()
    {
        RuleFor(x => x.CashierId)
            .NotEmpty()
            .WithMessage("Cashier ID is required.");

        RuleFor(x => x.OpeningFloat)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Opening float must be greater than or equal to 0.");

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .WithMessage("Notes must not exceed 500 characters.")
            .When(x => x.Notes != null);
    }
}
