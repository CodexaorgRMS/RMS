using FluentValidation;

namespace Inventory.Application.Features.InventoryItems.Commands.IncreaseQuantity;

public sealed class IncreaseInventoryItemQuantityCommandValidator
    : AbstractValidator<IncreaseInventoryItemQuantityCommand>
{
    public IncreaseInventoryItemQuantityCommandValidator()
    {
        RuleFor(x => x.InventoryItemId)
            .NotEmpty()
            .WithMessage("Inventory item id is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");
    }
}