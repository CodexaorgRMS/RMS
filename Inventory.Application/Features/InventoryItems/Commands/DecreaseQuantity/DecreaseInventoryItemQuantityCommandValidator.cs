using FluentValidation;
using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Features.InventoryItems.Commands.DecreaseQuantity;

public sealed class DecreaseInventoryItemQuantityCommandValidator
    : AbstractValidator<DecreaseInventoryItemQuantityCommand>
{
    private readonly IInventoryDataContext _context;

    public DecreaseInventoryItemQuantityCommandValidator(IInventoryDataContext context)
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

        RuleFor(x => x)
            .MustAsync(HasSufficientStock)
            .WithMessage("Insufficient stock.");
    }

    private async Task<bool> InventoryItemExists(Guid inventoryItemId, CancellationToken cancellationToken)
    {
        return await _context.InventoryItems.AnyAsync(x => x.InventoryItemId == inventoryItemId, cancellationToken);
    }

    private async Task<bool> HasSufficientStock(DecreaseInventoryItemQuantityCommand command, CancellationToken cancellationToken)
    {
        return await _context.InventoryItems
            .AnyAsync(x => x.InventoryItemId == command.InventoryItemId && x.Quantity >= command.Quantity, cancellationToken);
    }
}
