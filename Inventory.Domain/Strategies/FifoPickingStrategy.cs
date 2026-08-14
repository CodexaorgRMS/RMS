using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;

namespace Inventory.Domain.Strategies
{
	public class FifoPickingStrategy : IInventoryPickingStrategy
	{
		public IEnumerable<ProductBatch> SortBatches(IEnumerable<ProductBatch> availableBatches)
		{
			return availableBatches.OrderBy(b => b.CreatedAt);
		}
	}
}
