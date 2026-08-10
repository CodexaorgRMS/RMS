using FluentResults;
using Inventory.Application.Abstractions;
using Inventory.Domain.Entities;
using SharedContracts.Inventory.Events;
using Wolverine;
using Wolverine.Attributes;

namespace Inventory.Application.Features.InventoryItems.Commands.Create;

[Transactional]
public static class CreateInventoryItemHandler
{
    public static async Task<Result<Guid>> Handle(
        CreateInventoryItemCommand command,
        IInventoryDbContext context,
        IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var inventoryItem = new InventoryItem
        {
            InventoryItemId = Guid.NewGuid(),
            ProductId = command.ProductId,
            Quantity = command.Quantity,
            MinStock = command.MinStock,
            UpdatedAt = DateTime.UtcNow
        };

        await context.InventoryItems.AddAsync(
            inventoryItem,
            cancellationToken);

        if (inventoryItem.Quantity == 0)
        {
            await bus.PublishAsync(
                new InventoryItemOutOfStockEvent(
                    inventoryItem.InventoryItemId,
                    inventoryItem.ProductId,
                    DateTime.UtcNow));
        }
        else if (inventoryItem.Quantity <= inventoryItem.MinStock)
        {
            await bus.PublishAsync(
                new LowStockDetectedEvent(
                    inventoryItem.InventoryItemId,
                    inventoryItem.ProductId,
                    inventoryItem.Quantity,
                    inventoryItem.MinStock,
                    DateTime.UtcNow));
        }

        return Result.Ok(inventoryItem.InventoryItemId);
    }
}