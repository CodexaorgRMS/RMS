using FluentResults;
using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Inventory.Events;
using Wolverine;
using Wolverine.Attributes;

namespace Inventory.Application.Features.InventoryItems.Commands.IncreaseQuantity;

[Transactional]
public static class IncreaseInventoryItemQuantityHandler
{
    public static async Task<Result> Handle(
        IncreaseInventoryItemQuantityCommand command,
        IInventoryDbContext context,
        IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var inventoryItem = await context.InventoryItems
            .FirstOrDefaultAsync(
                x => x.InventoryItemId == command.InventoryItemId,
                cancellationToken);

        if (inventoryItem is null)
        {
            return Result.Fail("Inventory item not found.");
        }

        var previousQuantity = inventoryItem.Quantity;

        var wasOutOfStock =
            previousQuantity == 0;

        var wasLowStock =
            previousQuantity > 0 &&
            previousQuantity <= inventoryItem.MinStock;

        inventoryItem.Quantity += command.Quantity;
        inventoryItem.UpdatedAt = DateTime.UtcNow;

        var isNormalStock =
            inventoryItem.Quantity > inventoryItem.MinStock;

        // Out Of Stock -> Has Stock
        if (wasOutOfStock && inventoryItem.Quantity > 0)
        {
            await bus.PublishAsync(
                new InventoryItemRestockedEvent(
                    inventoryItem.InventoryItemId,
                    inventoryItem.ProductId,
                    inventoryItem.Quantity,
                    inventoryItem.MinStock,
                    DateTime.UtcNow));
        }

        // Low Stock -> Normal
        if (wasLowStock && isNormalStock)
        {
            await bus.PublishAsync(
                new InventoryItemStockRecoveredEvent(
                    inventoryItem.InventoryItemId,
                    inventoryItem.ProductId,
                    inventoryItem.Quantity,
                    inventoryItem.MinStock,
                    DateTime.UtcNow));
        }

        return Result.Ok();
    }
}