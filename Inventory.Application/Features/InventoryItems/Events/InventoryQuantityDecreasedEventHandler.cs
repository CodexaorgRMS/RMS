using Inventory.Application.Abstractions;
using Inventory.Domain.Entities;
using Wolverine.Attributes;

namespace Inventory.Application.Features.InventoryItems.Events;

[Transactional]
public static class InventoryQuantityDecreasedEventHandler
{
    public static async Task Handle(
        InventoryQuantityDecreasedEvent @event,
        IInventoryDataContext context,
        CancellationToken cancellationToken)
    {
        var movement = new StockMovement
        {
            MovementId = Guid.NewGuid(),
            ProductId = @event.ProductId,
            Type = "Decrease",
            Quantity = @event.Quantity,
            ReferenceId = @event.InventoryItemId,
            CreatedAt = @event.OccurredAt
        };

        await context.StockMovements.AddAsync(
            movement,
            cancellationToken);
    }
}