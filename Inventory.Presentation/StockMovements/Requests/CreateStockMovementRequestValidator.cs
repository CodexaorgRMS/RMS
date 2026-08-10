using FluentValidation;

namespace Inventory.Presentation.StockMovements.Requests;

public sealed class CreateStockMovementRequestValidator
    : AbstractValidator<CreateStockMovementRequest>
{
    public CreateStockMovementRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product id is required.");

        RuleFor(x => x.Type)
            .NotEmpty()
            .WithMessage("Type is required.")
            .Must(t => t is "IN" or "OUT" or "ADJUST")
            .WithMessage("Type must be IN, OUT, or ADJUST.");

        RuleFor(x => x.Quantity)
            .NotEmpty()
            .WithMessage("Quantity is required and cannot be zero.");

        RuleFor(x => x.ReferenceId)
            .NotEmpty()
            .WithMessage("Reference id is required.");
    }
}