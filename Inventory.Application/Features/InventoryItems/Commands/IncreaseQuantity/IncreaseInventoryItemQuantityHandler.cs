using FluentResults;
using Inventory.Application.Abstractions;
using Inventory.Application.Features.InventoryItems.Events;
using Inventory.Domain.Entities;
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
            .FirstAsync(
                x => x.InventoryItemId == command.InventoryItemId,
                cancellationToken);

        var previousQuantity = inventoryItem.Quantity;
        var wasOutOfStock = previousQuantity == 0;

        inventoryItem.Quantity += command.Quantity;
        inventoryItem.UpdatedAt = DateTime.UtcNow;

        await bus.PublishAsync(
            new InventoryStockmovementEvent(
                inventoryItem.InventoryItemId,
                inventoryItem.ProductId,
                StockMovementType.In,
                command.Quantity,
                DateTime.UtcNow));

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