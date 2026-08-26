using Finance.Domain.Enums;
using FluentValidation;

namespace Finance.Presentation.Requests.Validators;

public sealed class RecordCashMovementRequestValidator : AbstractValidator<RecordCashMovementRequest>
{
    public RecordCashMovementRequestValidator()
    {
        RuleFor(x => x.Amount)
            .GreaterThan(0)
            .WithMessage("Amount must be greater than 0.");

        RuleFor(x => x.Description)
            .NotEmpty()
            .WithMessage("Description is required.")
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters.");

        RuleFor(x => x.Type)
            .Must(t => Enum.IsDefined(typeof(CashMovementType), t))
            .WithMessage("Valid cash movement type is required.");

        RuleFor(x => x.Source)
            .Must(s => Enum.IsDefined(typeof(PaymentSource), s))
            .WithMessage("Valid payment source is required.");

        RuleFor(x => x.CreatedBy)
            .NotEmpty()
            .WithMessage("CreatedBy user ID is required.");
    }
}
