using FluentValidation;
using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Features.InventoryItems.Commands.UpdateMinStock;

public sealed class UpdateInventoryItemMinStockCommandValidator
    : AbstractValidator<UpdateInventoryItemMinStockCommand>
{
    private readonly IInventoryDataContext _context;

    public UpdateInventoryItemMinStockCommandValidator(IInventoryDataContext context)
    {
        _context = context;

        RuleFor(x => x.InventoryItemId)
            .NotEmpty()
            .WithMessage("Inventory item id is required.")
            .MustAsync(InventoryItemExists)
            .WithMessage("Inventory item not found.");

        RuleFor(x => x.MinStock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum stock cannot be negative.");
    }

    private async Task<bool> InventoryItemExists(Guid inventoryItemId, CancellationToken cancellationToken)
    {
        return await _context.InventoryItems.AnyAsync(x => x.InventoryItemId == inventoryItemId, cancellationToken);
    }
}