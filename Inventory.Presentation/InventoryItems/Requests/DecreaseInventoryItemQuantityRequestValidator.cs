using FluentValidation;

namespace Inventory.Presentation.InventoryItems.Requests;

public sealed class DecreaseInventoryItemQuantityRequestValidator
    : AbstractValidator<DecreaseInventoryItemQuantityRequest>
{
    public DecreaseInventoryItemQuantityRequestValidator()
    {
        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");
    }
}
