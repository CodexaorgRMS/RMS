using FluentValidation;

namespace Inventory.Presentation.InventoryItems.Requests;

public sealed class IncreaseInventoryItemQuantityRequestValidator
    : AbstractValidator<IncreaseInventoryItemQuantityRequest>
{
    public IncreaseInventoryItemQuantityRequestValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");
    }
}