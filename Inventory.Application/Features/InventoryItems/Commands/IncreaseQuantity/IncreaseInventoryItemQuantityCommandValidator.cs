using FluentValidation;
using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Features.InventoryItems.Commands.IncreaseQuantity;

public sealed class IncreaseInventoryItemQuantityCommandValidator
    : AbstractValidator<IncreaseInventoryItemQuantityCommand>
{
    private readonly IInventoryDataContext _context;

    public IncreaseInventoryItemQuantityCommandValidator(IInventoryDataContext context)
    {
        _context = context;

        RuleFor(x => x.InventoryItemId)
            .NotEmpty()
            .WithMessage("Inventory item id is required.")
            .MustAsync(InventoryItemExists)
            .WithMessage("Inventory item not found.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .WithMessage("Quantity must be greater than zero.");
    }

    private async Task<bool> InventoryItemExists(Guid inventoryItemId, CancellationToken cancellationToken)
    {
        return await _context.InventoryItems.AnyAsync(x => x.InventoryItemId == inventoryItemId, cancellationToken);
    }
}