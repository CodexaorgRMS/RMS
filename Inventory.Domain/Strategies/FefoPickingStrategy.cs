using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;

namespace Inventory.Domain.Strategies
{
	public class FefoPickingStrategy : IInventoryPickingStrategy
	{
		public IEnumerable<ProductBatch> SortBatches(IEnumerable<ProductBatch> availableBatches)
		{
			return availableBatches
				.OrderBy(b => b.ExpiryDate)
				.ThenBy(b => b.CreatedAt);
		}
	}
}
