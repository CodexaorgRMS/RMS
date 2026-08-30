using Inventory.Application.Abstractions;
using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Inventory.Commands;
using SharedContracts.Sales.Saga;
using Wolverine.Attributes;

namespace Inventory.Application.Features.Stocks.Saga;

/// <summary>
/// Compensation handler: restores all previously deducted stock for a failed checkout.
/// Uses the existing RestoreStockCommand internally to reverse outbound sale movements
/// back to their original batches.
/// </summary>
public static class CompensateCheckoutStockHandler
{
	[Transactional]
	public static async Task Handle(
		CompensateCheckoutStock command,
		IInventoryDataContext context,
		CancellationToken cancellationToken)
	{
		foreach (var item in command.DeductedItems)
		{
			// Find all outbound sale movements created for this order + product
			var outboundMovements = await context.StockMovements
				.Where(m =>
					m.ProductId == item.ProductId &&
					m.ReferenceId == command.OrderId &&
					m.Type == StockMovementType.Outbound_Sale)
				.ToListAsync(cancellationToken);

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

				// Record the inbound compensation movement
				var returnMovement = new StockMovement
				{
					MovementId = Guid.NewGuid(),
					ProductId = item.ProductId,
					ProductBatchId = original.ProductBatchId,
					Type = StockMovementType.Inbound_Return,
					Quantity = restoredQuantity,
					ReferenceId = command.OrderId,
					CreatedAt = DateTime.UtcNow
				};

				await context.StockMovements.AddAsync(returnMovement, cancellationToken);
			}

			// Restore the inventory item aggregate quantity
			var inventoryItem = await context.InventoryItems
				.FirstOrDefaultAsync(i => i.ProductId == item.ProductId, cancellationToken);

			if (inventoryItem is not null)
			{
				inventoryItem.Quantity += item.Quantity;
			}
		}
	}
}
