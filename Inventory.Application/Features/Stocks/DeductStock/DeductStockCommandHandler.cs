using FluentResults;
using Inventory.Application.Abstractions;
using Inventory.Domain.Entities;
using Inventory.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Inventory.Commands;
using Wolverine.Attributes;

namespace Inventory.Application.Features.Stock.Commands.DeductStock;

public static class DeductStockCommandHandler
{
	 [Transactional] 
	public static async Task<Result> Handle(
		DeductStockCommand command,
		IInventoryDataContext context,
		IPickingStrategyFactory strategyFactory,
		CancellationToken cancellationToken)
	{
        var product = await context.Products
            .Include(p => p.Category)
            .Include(p => p.ProductBatches.Where(b =>
                b.Status == BatchStatus.Active &&
                b.ExpiryDate > DateTime.UtcNow &&
                b.CurrentQuantity > 0))
            .FirstOrDefaultAsync(p => p.ProductId == command.ProductId, cancellationToken);

        if (product == null)
			return Result.Fail($"Product  not found.");

		
		var totalAvailableStock = product.ProductBatches.Sum(b => b.CurrentQuantity);
		if (totalAvailableStock < command.Quantity)
			return Result.Fail($"Insufficient stock. Requested: {command.Quantity}, Available: {totalAvailableStock}");


		var strategyEnum = product.CustomPickingStrategy ?? product.Category.DefaultPickingStrategy;
		var pickingStrategy = strategyFactory.GetStrategy(strategyEnum);


		var sortedBatches = pickingStrategy.SortBatches(product.ProductBatches).ToList();

		int remainingToDeduct = command.Quantity;


		foreach (var batch in sortedBatches)
		{
			if (remainingToDeduct <= 0)
				break; 

		
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

		var inventoryItem = await context.InventoryItems
	           .FirstOrDefaultAsync(i => i.ProductId == command.ProductId, cancellationToken);

		if (inventoryItem != null)
		{
			inventoryItem.Quantity -= command.Quantity;
		}


		return Result.Ok();
	}
}