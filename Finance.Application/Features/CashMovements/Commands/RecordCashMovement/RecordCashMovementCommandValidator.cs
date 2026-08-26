using FluentValidation;

namespace Finance.Application.Features.CashMovements.Commands.RecordCashMovement;

public sealed class RecordCashMovementCommandValidator : AbstractValidator<RecordCashMovementCommand>
{
    public RecordCashMovementCommandValidator()
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
            .IsInEnum()
            .WithMessage("Valid cash movement type is required.");

        RuleFor(x => x.Source)
            .IsInEnum()
            .WithMessage("Valid payment source is required.");

        RuleFor(x => x.CreatedBy)
            .NotEmpty()
            .WithMessage("CreatedBy user ID is required.");
    }
}
