using FluentValidation;

namespace Inventory.Presentation.InventoryItems.Requests;

public sealed class UpdateInventoryItemMinStockRequestValidator
    : AbstractValidator<UpdateInventoryItemMinStockRequest>
{
    public UpdateInventoryItemMinStockRequestValidator()
    {
        RuleFor(x => x.MinStock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum stock cannot be negative.");
    }
}
