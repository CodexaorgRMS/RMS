using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Domain.Strategies
{


	public class HifoPickingStrategy : IInventoryPickingStrategy
	{
		public IEnumerable<ProductBatch> SortBatches(IEnumerable<ProductBatch> availableBatches)
		{
			return availableBatches.OrderByDescending(b => b.CostPrice);
		}
	}
}
