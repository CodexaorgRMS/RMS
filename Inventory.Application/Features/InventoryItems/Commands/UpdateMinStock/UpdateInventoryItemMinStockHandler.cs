using FluentResults;
using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Inventory.Events;
using Wolverine;
using Wolverine.Attributes;

namespace Inventory.Application.Features.InventoryItems.Commands.UpdateMinStock;

[Transactional]
public static class UpdateInventoryItemMinStockHandler
{
    public static async Task<Result<UpdateInventoryItemMinStockResult>> Handle(
        UpdateInventoryItemMinStockCommand command,
        IInventoryDataContext context,
        IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var inventoryItem = await context.InventoryItems
            .FirstAsync(
                x => x.InventoryItemId == command.InventoryItemId,
                cancellationToken);

        var wasLowStock =
            inventoryItem.Quantity > 0 &&
            inventoryItem.Quantity <= inventoryItem.MinStock;

        inventoryItem.MinStock = command.MinStock;
        inventoryItem.UpdatedAt = DateTime.UtcNow;

        var isLowStock =
            inventoryItem.Quantity > 0 &&
            inventoryItem.Quantity <= inventoryItem.MinStock;

        if (!wasLowStock && isLowStock)
        {
            await bus.PublishAsync(
                new LowStockDetectedIntegrationEvent(
                    inventoryItem.InventoryItemId,
                    inventoryItem.ProductId,
                    inventoryItem.Quantity,
                    inventoryItem.MinStock,
                    DateTime.UtcNow));
        }

        return Result.Ok(
           new UpdateInventoryItemMinStockResult(
               inventoryItem.InventoryItemId,
               inventoryItem.Quantity,
               inventoryItem.MinStock));
    }
}