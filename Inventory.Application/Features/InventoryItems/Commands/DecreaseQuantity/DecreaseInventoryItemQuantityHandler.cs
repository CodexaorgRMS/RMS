using FluentResults;
using Inventory.Application.Abstractions;
using Inventory.Application.Features.InventoryItems.Events;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Inventory.Events;
using Wolverine;
using Wolverine.Attributes;

namespace Inventory.Application.Features.InventoryItems.Commands.DecreaseQuantity;

[Transactional]
public static class DecreaseInventoryItemQuantityHandler
{
    public static async Task<Result<DecreaseInventoryItemQuantityResult>> Handle(
        DecreaseInventoryItemQuantityCommand command,
        IInventoryDataContext context,
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

        if (inventoryItem.Quantity < command.Quantity)
        {
            return Result.Fail("Insufficient stock.");
        }

        var previousQuantity = inventoryItem.Quantity;

        var wasLowStock =
            previousQuantity > 0 &&
            previousQuantity <= inventoryItem.MinStock;

        var wasOutOfStock =
            previousQuantity == 0;

        inventoryItem.Quantity -= command.Quantity;
        inventoryItem.UpdatedAt = DateTime.UtcNow;

        await bus.PublishAsync(
    new InventoryQuantityDecreasedEvent(
        inventoryItem.InventoryItemId,
        inventoryItem.ProductId,
        command.Quantity,
        DateTime.UtcNow));


        var isOutOfStock =
            inventoryItem.Quantity == 0;

        var isLowStock =
            inventoryItem.Quantity > 0 &&
            inventoryItem.Quantity <= inventoryItem.MinStock;

        // Any stocked state -> Out Of Stock
        if (!wasOutOfStock && isOutOfStock)
        {
            await bus.PublishAsync(
                new InventoryItemOutOfStockIntegrationEvent(
                    inventoryItem.InventoryItemId,
                    inventoryItem.ProductId,
                    DateTime.UtcNow));
        }

        // Normal -> Low Stock
        else if (!wasLowStock && isLowStock)
        {
            await bus.PublishAsync(
                new LowStockDetectedIntegrationEvent(
                    inventoryItem.InventoryItemId,
                    inventoryItem.ProductId,
                    inventoryItem.Quantity,
                    inventoryItem.MinStock,
                    DateTime.UtcNow));
        }

        return Result.Ok(new DecreaseInventoryItemQuantityResult(
            inventoryItem.InventoryItemId,
            inventoryItem.Quantity,
            inventoryItem.MinStock));
    }
}