using FluentResults;
using Inventory.Application.Abstractions;
using Inventory.Domain.Entities;
using Inventory.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Inventory.Commands;
using SharedContracts.Sales.Saga;
using Wolverine;
using Wolverine.Attributes;

namespace Inventory.Application.Features.Stocks.Saga;

/// <summary>
/// Handles the inventory deduction step within the Checkout Saga.
/// Deducts stock for ALL order items inside a single [Transactional] boundary.
/// If any item fails, the entire transaction is rolled back automatically by EF Core
/// and a failure response is returned to the orchestrator.
/// </summary>
public static class DeductStockForCheckoutHandler
{
	[Transactional]
	public static async Task<CheckoutStockDeducted> Handle(
		DeductStockForCheckout command,
		IInventoryDataContext context,
		IPickingStrategyFactory strategyFactory,
		CancellationToken cancellationToken)
	{
		var errors = new List<string>();

		foreach (var item in command.Items)
		{
			var product = await context.Products
				.Include(p => p.Category)
				.Include(p => p.ProductBatches.Where(b =>
					b.Status == BatchStatus.Active &&
					b.ExpiryDate > DateTime.UtcNow &&
					b.CurrentQuantity > 0))
				.FirstOrDefaultAsync(p => p.ProductId == item.ProductId, cancellationToken);

			if (product is null)
			{
				errors.Add($"Product '{item.ProductId}' not found.");
				continue;
			}

			var totalAvailableStock = product.ProductBatches.Sum(b => b.CurrentQuantity);
			if (totalAvailableStock < item.Quantity)
			{
				errors.Add(
					$"Insufficient stock for product '{product.ProductId}'. " +
					$"Requested: {item.Quantity}, Available: {totalAvailableStock}");
				continue;
			}

			// ── Pick batches using the configured strategy ─────────────────
			var strategyEnum = product.CustomPickingStrategy ?? product.Category.DefaultPickingStrategy;
			var pickingStrategy = strategyFactory.GetStrategy(strategyEnum);
			var sortedBatches = pickingStrategy.SortBatches(product.ProductBatches).ToList();

			int remainingToDeduct = item.Quantity;

			foreach (var batch in sortedBatches)
			{
				if (remainingToDeduct <= 0) break;

				int amountToDeductFromBatch = Math.Min(batch.CurrentQuantity, remainingToDeduct);

				batch.CurrentQuantity -= amountToDeductFromBatch;
				remainingToDeduct -= amountToDeductFromBatch;

				var movement = new StockMovement
				{
					MovementId = Guid.NewGuid(),
					ProductId = product.ProductId,
					ProductBatchId = batch.BatchId,
					Type = StockMovementType.Outbound_Sale,
					Quantity = -amountToDeductFromBatch,
					ReferenceId = command.OrderId,
					CreatedAt = DateTime.UtcNow
				};

				await context.StockMovements.AddAsync(movement, cancellationToken);
			}

			// ── Update the inventory summary ───────────────────────────────
			var inventoryItem = await context.InventoryItems
				.FirstOrDefaultAsync(i => i.ProductId == item.ProductId, cancellationToken);

			if (inventoryItem is not null)
			{
				inventoryItem.Quantity -= item.Quantity;
			}
			else
			{
				errors.Add($"Inventory item for product '{item.ProductId}' not found.");
			}
		}

		// If ANY item failed, the [Transactional] attribute will cause EF Core
		// to NOT commit, since we return a failure before the middleware saves.
		if (errors.Count > 0)
		{
			// Throw to trigger automatic EF Core rollback inside the transaction
			throw new InvalidOperationException(
				$"Checkout saga stock deduction failed: {string.Join(" | ", errors)}");
		}

		return new CheckoutStockDeducted(command.SagaId, true);
	}
}
