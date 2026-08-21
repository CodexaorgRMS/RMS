using FluentResults;
using Inventory.Application.Abstractions;
using Inventory.Application.Features.ProductBatches.Events;
using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Wolverine;
using Wolverine.Attributes;

namespace Inventory.Application.Features.ProductBatches.Commands.Receive
{
    [Transactional]
    public static class ReceiveProductBatchHandler
    {
        public static async Task<Result<Guid>> Handle(
            ReceiveProductBatchCommand command,
            IInventoryDataContext context,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            // 1) Create Product Batch
            var batch = new ProductBatch
            {
                BatchId = Guid.NewGuid(),
                ProductId = command.ProductId,
                CostPrice = command.CostPrice,
                InitialQuantity = command.Quantity,
                CurrentQuantity = command.Quantity,
                ExpiryDate = command.ExpiryDate,
                CreatedAt = DateTime.UtcNow
            };

            await context.ProductBatches.AddAsync(
                batch,
                cancellationToken);


            // 2) Update Inventory Item
            var inventoryItem = await context.InventoryItems
                .FirstOrDefaultAsync(
                    x => x.ProductId == command.ProductId,
                    cancellationToken);

            if (inventoryItem is null)
            {
                inventoryItem = new InventoryItem
                {
                    InventoryItemId = Guid.NewGuid(),
                    ProductId = command.ProductId,
                    Quantity = command.Quantity,
                    MinStock = 0,
                    UpdatedAt = DateTime.UtcNow
                };

                await context.InventoryItems.AddAsync(
                    inventoryItem,
                    cancellationToken);
            }
            else
            {
                inventoryItem.Quantity += command.Quantity;
                inventoryItem.UpdatedAt = DateTime.UtcNow;
            }


            // 3) Create Stock Movement
            var stockMovement = new StockMovement
            {
                MovementId = Guid.NewGuid(),
                ProductId = command.ProductId,
                ProductBatchId = batch.BatchId,
                Type = StockMovementType.In,
                Quantity = command.Quantity,
                ReferenceId = command.ReferenceId,
                CreatedAt = DateTime.UtcNow
            };

            await context.StockMovements.AddAsync(
                stockMovement,
                cancellationToken);


            // Optional Event
            var @event = new ProductBatchReceivedEvent(
                batch.BatchId,
                batch.ProductId,
                batch.CostPrice,
                batch.InitialQuantity,
                batch.ExpiryDate,
                batch.CreatedAt,
                command.ReferenceId);

            await bus.PublishAsync(@event);


            return Result.Ok(batch.BatchId);
        }
    }
}