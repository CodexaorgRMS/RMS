using FluentResults;
using Inventory.Application.Abstractions;
using Inventory.Application.Features.InventoryItems.Events;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Inventory.Events;
using Wolverine;
using Wolverine.Attributes;

namespace Inventory.Application.Features.InventoryItems.Commands.IncreaseQuantity;

[Transactional]
public static class IncreaseInventoryItemQuantityHandler
{
    public static async Task<Result<IncreaseInventoryItemQuantityResult>> Handle(
        IncreaseInventoryItemQuantityCommand command,
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
            return Result.Fail<IncreaseInventoryItemQuantityResult>(
                "Inventory item not found.");
        }

        var previousQuantity = inventoryItem.Quantity;

        var wasOutOfStock = previousQuantity == 0;

        var wasLowStock =
            previousQuantity > 0 &&
            previousQuantity <= inventoryItem.MinStock;

        inventoryItem.Quantity += command.Quantity;
        inventoryItem.UpdatedAt = DateTime.UtcNow;

        await bus.PublishAsync(
    new InventoryQuantityIncreasedEvent(
        inventoryItem.InventoryItemId,
        inventoryItem.ProductId,
        command.Quantity,
        DateTime.UtcNow));

        var isNormalStock =
            inventoryItem.Quantity > inventoryItem.MinStock;

        // Out Of Stock -> Has Stock
        if (wasOutOfStock && inventoryItem.Quantity > 0)
        {
            await bus.PublishAsync(
                new InventoryItemRestockedIntegrationEvent(
                    inventoryItem.InventoryItemId,
                    inventoryItem.ProductId,
                    inventoryItem.Quantity,
                    inventoryItem.MinStock,
                    DateTime.UtcNow));
        }

        
        return Result.Ok(
            new IncreaseInventoryItemQuantityResult(
                inventoryItem.InventoryItemId,
                inventoryItem.Quantity,
                inventoryItem.MinStock));
    }
}