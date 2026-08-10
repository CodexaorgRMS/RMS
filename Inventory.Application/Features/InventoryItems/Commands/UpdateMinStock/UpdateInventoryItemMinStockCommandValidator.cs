using FluentValidation;

namespace Inventory.Application.Features.InventoryItems.Commands.UpdateMinStock;

public sealed class UpdateInventoryItemMinStockCommandValidator
    : AbstractValidator<UpdateInventoryItemMinStockCommand>
{
    public UpdateInventoryItemMinStockCommandValidator()
    {
        RuleFor(x => x.InventoryItemId)
            .NotEmpty()
            .WithMessage("Inventory item id is required.");

        RuleFor(x => x.MinStock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum stock cannot be negative.");
    }
}