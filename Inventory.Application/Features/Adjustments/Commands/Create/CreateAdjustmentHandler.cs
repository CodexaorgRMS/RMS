using FluentResults;
using Inventory.Application.Abstractions;
using Inventory.Application.Features.InventoryItems.Events;
using Inventory.Domain.Entities;
using JasperFx.Events;
using Microsoft.EntityFrameworkCore;
using Wolverine;
using Wolverine.Attributes;

namespace Inventory.Application.Features.Adjustments.Commands.Create
{
    [Transactional]
    public static class CreateAdjustmentHandler
    {
        public static async Task<Result<Guid>> Handle(
            CreateAdjustmentCommand command,
            IInventoryDataContext context,
            IMessageBus bus,
            CancellationToken cancellationToken)
        {
            var batch = await context.ProductBatches
                .FirstAsync(b => b.BatchId == command.ProductBatchId, cancellationToken);

            if (command.Type == AdjustmentType.Increase)
            {
                batch.CurrentQuantity += command.Quantity;
            }
            else
            {
                batch.CurrentQuantity -= command.Quantity;
            }

            var totalFinancialImpact = command.Quantity * batch.CostPrice;

            var adjustment = new Adjustment
            {
                ProductId = command.ProductId,
                ProductBatchId = command.ProductBatchId,
                Type = command.Type,
                Reason = command.Reason,
                Quantity = command.Quantity,
                TotalFinancialImpact = totalFinancialImpact,
                Note = command.Note,
                CreatedAt = DateTime.UtcNow
            };

            await context.Adjustments.AddAsync(adjustment, cancellationToken);


            var inventoryItem = await context.InventoryItems
                .FirstOrDefaultAsync(i => i.ProductId == command.ProductId, cancellationToken);

            if (inventoryItem == null)
            {
                return Result.Fail(new Error($"Inventory item with ProductId {command.ProductId} not found."));
			}

            inventoryItem.Quantity = batch.CurrentQuantity;

			var eventType = command.Type == AdjustmentType.Increase ? StockMovementType.In : StockMovementType.Out;


			var movement = new StockMovement
			{
                ProductId = command.ProductId,
                ProductBatchId = command.ProductBatchId,
                Quantity = command.Quantity,
				Type = eventType,
                CreatedAt = DateTime.UtcNow, 
                ReferenceId = adjustment.AdjustmentId
			};

			await context.StockMovements.AddAsync(
		  movement,
		  cancellationToken);
			await context.SaveChangesAsync(cancellationToken);

			return Result.Ok(adjustment.AdjustmentId);
        }
    }
}
