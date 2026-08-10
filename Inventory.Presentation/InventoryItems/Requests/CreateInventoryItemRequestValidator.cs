using FluentValidation;

namespace Inventory.Presentation.InventoryItems.Requests;

public sealed class CreateInventoryItemRequestValidator
    : AbstractValidator<CreateInventoryItemRequest>
{
    public CreateInventoryItemRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product id is required.");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Quantity cannot be negative.");

        RuleFor(x => x.MinStock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum stock cannot be negative.");
    }
}