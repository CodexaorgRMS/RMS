using FluentResults;
using Inventory.Application.Abstractions;
using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Inventory.Commands;
using Wolverine.Attributes;

namespace Inventory.Application.Features.Stocks.RestoreStock;

public static class RestoreStockCommandHandler
{
	[Transactional]
	public static async Task<Result> Handle(
		RestoreStockCommand command,
		IInventoryDataContext context,
		CancellationToken cancellationToken)
	{
		// Find the original outbound movements for this order and product
		var outboundMovements = await context.StockMovements
			.Where(m =>
				m.ProductId == command.ProductId &&
				m.ReferenceId == command.OrderId &&
				m.Type == StockMovementType.Outbound_Sale)
			.ToListAsync(cancellationToken);

		if (!outboundMovements.Any())
			return Result.Fail($"No outbound sale movements found for product {command.ProductId} on order {command.OrderId}.");

		// Reverse each outbound movement back to its original batch
		foreach (var original in outboundMovements)
		{
			int restoredQuantity = Math.Abs(original.Quantity);

			// Restore to the original batch
			var batch = await context.ProductBatches
				.FirstOrDefaultAsync(b => b.BatchId == original.ProductBatchId, cancellationToken);

			if (batch is not null)
			{
				batch.CurrentQuantity += restoredQuantity;
			}

			// Record the inbound return movement
			var returnMovement = new StockMovement
			{
				MovementId = Guid.NewGuid(),
				ProductId = command.ProductId,
				ProductBatchId = original.ProductBatchId,
				Type = StockMovementType.Inbound_Return,
				Quantity = restoredQuantity,
				ReferenceId = command.OrderId,
				CreatedAt = DateTime.UtcNow
			};

			await context.StockMovements.AddAsync(returnMovement, cancellationToken);
		}

		// Update the InventoryItem aggregate quantity
		var inventoryItem = await context.InventoryItems
			.FirstOrDefaultAsync(i => i.ProductId == command.ProductId, cancellationToken);

		if (inventoryItem is not null)
		{
			inventoryItem.Quantity += command.Quantity;
		}

		return Result.Ok();
	}
}
