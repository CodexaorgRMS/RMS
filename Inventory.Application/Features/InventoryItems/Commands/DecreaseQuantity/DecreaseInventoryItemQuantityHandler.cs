using FluentResults;
using Inventory.Application.Abstractions;
using Inventory.Application.Features.InventoryItems.Events;
using Inventory.Domain.Entities;
using Inventory.Domain.Enums;
using Inventory.Domain.Interfaces;
using Inventory.Domain.Strategies; // ???? ??? Strategies
using Microsoft.EntityFrameworkCore;
using SharedContracts.Inventory.Events;
using Wolverine;
using Wolverine.Attributes;

namespace Inventory.Application.Features.InventoryItems.Commands.DecreaseQuantity;

[Transactional]
public static class DecreaseInventoryItemQuantityHandler
{
	public static async Task<Result<DecreaseInventoryItemQuantityResult>> Handle(
		DecreaseInventoryItemQuantityCommand command,
		IInventoryDataContext context,
		IMessageBus bus,
		CancellationToken cancellationToken)
	{
		var inventoryItem = await context.InventoryItems
			.Include(i => i.Product)
			.ThenInclude(p => p.ProductBatches.Where(b => b.CurrentQuantity > 0 && b.Status == BatchStatus.Active))
			.FirstAsync(x => x.InventoryItemId == command.InventoryItemId, cancellationToken);

		var previousQuantity = inventoryItem.Quantity;

		if (previousQuantity < command.Quantity)
		{
			return Result.Fail("out of stock");
		}

		IInventoryPickingStrategy pickingStrategy = new FefoPickingStrategy();
		var sortedBatches = pickingStrategy.SortBatches(inventoryItem.Product.ProductBatches).ToList();

		int remainingToDeduct = command.Quantity;

		foreach (var batch in sortedBatches)
		{
			if (remainingToDeduct <= 0) break;

			int deductAmount = Math.Min(batch.CurrentQuantity, remainingToDeduct);

			batch.CurrentQuantity -= deductAmount;
			remainingToDeduct -= deductAmount;

			await bus.PublishAsync(
				new InventoryStockmovementEvent(
					inventoryItem.InventoryItemId,
					inventoryItem.ProductId,
					batch.BatchId,
					StockMovementType.Outbound_Sale,
					deductAmount,
					DateTime.UtcNow));
		}

		if (remainingToDeduct > 0)
		{
			return Result.Fail("out of stoch in product batch");
		}

		var wasLowStock = previousQuantity > 0 && previousQuantity <= inventoryItem.MinStock;
		var wasOutOfStock = previousQuantity == 0;

		inventoryItem.Quantity -= command.Quantity;
		inventoryItem.UpdatedAt = DateTime.UtcNow;

		var isOutOfStock = inventoryItem.Quantity == 0;
		var isLowStock = inventoryItem.Quantity > 0 && inventoryItem.Quantity <= inventoryItem.MinStock;

		if (!wasOutOfStock && isOutOfStock)
		{
			await bus.PublishAsync(
				new InventoryItemOutOfStockIntegrationEvent(
					inventoryItem.InventoryItemId,
					inventoryItem.ProductId,
					DateTime.UtcNow));
		}
		else if (!wasLowStock && isLowStock)
		{
			await bus.PublishAsync(
				new LowStockDetectedIntegrationEvent(
					inventoryItem.InventoryItemId,
					inventoryItem.ProductId,
					inventoryItem.Quantity,
					inventoryItem.MinStock,
					DateTime.UtcNow));
		}

		return Result.Ok(new DecreaseInventoryItemQuantityResult(
			inventoryItem.InventoryItemId,
			inventoryItem.Quantity,
			inventoryItem.MinStock));
	}
}