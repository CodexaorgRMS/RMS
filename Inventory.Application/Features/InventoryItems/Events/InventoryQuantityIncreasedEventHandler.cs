using Inventory.Application.Abstractions;
using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Wolverine.Attributes;

namespace Inventory.Application.Features.InventoryItems.Events
{
    [Transactional]
    public static class InventoryQuantityIncreasedEventHandler
    {
        public static async Task Handle(InventoryQuantityIncreasedEvent @event, IInventoryDataContext context, CancellationToken cancellationToken)
        {
            var movement = new StockMovement
            {
                MovementId = Guid.NewGuid(),
                ProductId = @event.ProductId,
                Type = "Increase",
                Quantity = @event.Quantity,
                ReferenceId = @event.InventoryItemId,
                CreatedAt = @event.OccurredAt
            };

            await context.StockMovements.AddAsync(
          movement,
          cancellationToken);
        }
    }
}
