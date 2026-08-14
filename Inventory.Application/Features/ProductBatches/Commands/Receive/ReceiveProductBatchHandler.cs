using FluentResults;
using Inventory.Application.Abstractions;
using Inventory.Application.Features.ProductBatches.Events;
using Inventory.Domain.Entities;
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
            var batch = new ProductBatch
            {
                ProductId = command.ProductId,
                CostPrice = command.CostPrice,
                InitialQuantity = command.Quantity,
                CurrentQuantity = command.Quantity,
                ExpiryDate = command.ExpiryDate,
                CreatedAt = DateTime.UtcNow
            };

            await context.ProductBatches.AddAsync(batch, cancellationToken);

            var @event = new ProductBatchReceivedEvent(
                batch.BatchId,
                batch.ProductId,
                batch.CostPrice,
                batch.InitialQuantity,
                batch.ExpiryDate,
                batch.CreatedAt);

            await bus.PublishAsync(@event);

            return Result.Ok(batch.BatchId);
        }
    }
}
