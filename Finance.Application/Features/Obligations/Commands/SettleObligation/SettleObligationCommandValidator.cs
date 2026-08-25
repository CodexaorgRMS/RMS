using FluentValidation;

namespace Finance.Application.Features.Obligations.Commands.SettleObligation;

public sealed class SettleObligationCommandValidator : AbstractValidator<SettleObligationCommand>
{
    public SettleObligationCommandValidator()
    {
        RuleFor(x => x.ObligationId)
            .NotEmpty()
            .WithMessage("Obligation ID is required.");

        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Settlement amount must be greater than 0.");

        RuleFor(x => x.Source)
            .IsInEnum()
            .WithMessage("Valid payment source is required.");

        RuleFor(x => x.SettledBy)
            .NotEmpty()
            .WithMessage("SettledBy user ID is required.");

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .WithMessage("Notes must not exceed 500 characters.")
            .When(x => x.Notes != null);
    }
}
