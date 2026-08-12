using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Abstractions
{
	public interface IInventoryDataContext
	{
		DbSet<Product> Products { get; }

		DbSet<Category> Categories { get; }

		DbSet<InventoryItem> InventoryItems { get; }

		DbSet<StockMovement> StockMovements { get; }

		DbSet<ProductBatch> ProductBatches { get; }

		DbSet<Adjustment> Adjustments { get; }

		Task<int> SaveChangesAsync(
			CancellationToken cancellationToken = default);
	}
}
