using FluentValidation;

namespace Finance.Presentation.Requests.Validators;

public sealed class OpenShiftRequestValidator : AbstractValidator<OpenShiftRequest>
{
    public OpenShiftRequestValidator()
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

public sealed class CloseShiftRequestValidator : AbstractValidator<CloseShiftRequest>
{
    public CloseShiftRequestValidator()
    {
        RuleFor(x => x.ActualCash)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Actual cash must be greater than or equal to 0.");

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .WithMessage("Notes must not exceed 500 characters.")
            .When(x => x.Notes != null);
    }
}
