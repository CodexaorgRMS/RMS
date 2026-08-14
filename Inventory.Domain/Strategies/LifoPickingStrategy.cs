using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;

namespace Inventory.Domain.Strategies
{
	public class LifoPickingStrategy : IInventoryPickingStrategy
	{
		public IEnumerable<ProductBatch> SortBatches(IEnumerable<ProductBatch> availableBatches)
		{
			return availableBatches.OrderByDescending(b => b.CreatedAt);
		}
	}
}
