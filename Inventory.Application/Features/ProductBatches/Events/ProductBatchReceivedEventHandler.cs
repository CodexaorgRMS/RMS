using Inventory.Application.Abstractions;
using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Wolverine.Attributes;

namespace Inventory.Application.Features.ProductBatches.Events
{
    //[Transactional]
    public static class ProductBatchReceivedEventHandler
    {
        public static async Task Handle(
            ProductBatchReceivedEvent @event,
            IInventoryDataContext context,
            CancellationToken cancellationToken)
        {
            var movement = new StockMovement
            {
                ProductId = @event.ProductId,
                ProductBatchId = @event.BatchId,
                Type = StockMovementType.In,
                Quantity = @event.Quantity,
                ReferenceId = @event.BatchId,
                CreatedAt = @event.ReceivedAt
            };

            await context.StockMovements.AddAsync(movement, cancellationToken);

            var inventoryItem = await context.InventoryItems
                .FirstOrDefaultAsync(x => x.ProductId == @event.ProductId, cancellationToken);

            if (inventoryItem is not null)
            {
                inventoryItem.Quantity += @event.Quantity;
                inventoryItem.UpdatedAt = DateTime.UtcNow;
            }
            else
            {
                var newInventoryItem = new InventoryItem
                {
                    ProductId = @event.ProductId,
                    Quantity = @event.Quantity,
                    MinStock = 0,
                    UpdatedAt = DateTime.UtcNow
                };

                await context.InventoryItems.AddAsync(newInventoryItem, cancellationToken);
            }


            await context.SaveChangesAsync(cancellationToken);
		}
    }
}
