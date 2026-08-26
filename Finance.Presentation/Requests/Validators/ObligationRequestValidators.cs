using Finance.Domain.Enums;
using FluentValidation;

namespace Finance.Presentation.Requests.Validators;

public sealed class CreateObligationRequestValidator : AbstractValidator<CreateObligationRequest>
{
    public CreateObligationRequestValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Obligation title is required.")
            .MaximumLength(200)
            .WithMessage("Obligation title must not exceed 200 characters.");

        RuleFor(x => x.Type)
            .Must(t => Enum.IsDefined(typeof(ObligationType), t))
            .WithMessage("Valid obligation type is required.");

        RuleFor(x => x.Category)
            .Must(c => Enum.IsDefined(typeof(ObligationCategory), c))
            .WithMessage("Valid obligation category is required.");

        RuleFor(x => x.TotalAmount)
            .GreaterThan(0)
            .WithMessage("Total amount must be greater than 0.");

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .WithMessage("Notes must not exceed 500 characters.")
            .When(x => x.Notes != null);
    }
}

public sealed class SettleObligationRequestValidator : AbstractValidator<SettleObligationRequest>
{
    public SettleObligationRequestValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Settlement amount must be greater than 0.");

        RuleFor(x => x.Source)
            .Must(s => Enum.IsDefined(typeof(PaymentSource), s))
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
