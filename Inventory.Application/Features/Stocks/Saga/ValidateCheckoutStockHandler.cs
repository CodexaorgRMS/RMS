using Inventory.Application.Abstractions;
using Inventory.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Sales.Saga;

namespace Inventory.Application.Features.Stocks.Saga;

/// <summary>
/// Saga Step 1b: Validates stock availability for all checkout items.
/// Returns a <see cref="CheckoutStockValidated"/> response indicating
/// whether all items have sufficient stock.
/// </summary>
public static class ValidateCheckoutStockHandler
{
	public static async Task<CheckoutStockValidated> Handle(
		ValidateCheckoutStock command,
		IInventoryDataContext context,
		CancellationToken cancellationToken)
	{
		var errors = new List<string>();
		var enrichedItems = new List<EnrichedItemDto>();

		foreach (var item in command.Items)
		{
			var product = await context.Products
				.Include(p => p.ProductBatches)
				.FirstOrDefaultAsync(p => p.ProductId == item.ProductId, cancellationToken);

			if (product is null)
			{
				errors.Add($"Product '{item.ProductId}' not found.");
				continue;
			}

			var availableQuantity = product.ProductBatches
				.Where(b => b.Status == BatchStatus.Active && b.ExpiryDate > DateTime.UtcNow && b.CurrentQuantity > 0)
				.Sum(b => b.CurrentQuantity);

			if (availableQuantity < item.Quantity)
			{
				errors.Add(
					$"Insufficient stock for product {item.ProductId}. " +
					$"Requested: {item.Quantity}, Available: {availableQuantity}");
			}
			else
			{
				enrichedItems.Add(new EnrichedItemDto(product.ProductId, product.CategoryId));
			}
		}

		if (errors.Count > 0)
		{
			return new CheckoutStockValidated(
				command.SagaId,
				IsValid: false,
				ErrorMessage: string.Join(" | ", errors));
		}

		return new CheckoutStockValidated(command.SagaId, IsValid: true, EnrichedItems: enrichedItems);
	}
}
