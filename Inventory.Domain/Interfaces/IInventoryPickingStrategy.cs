using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Domain.Interfaces
{
	public interface IInventoryPickingStrategy
	{
		IEnumerable<ProductBatch> SortBatches(IEnumerable<ProductBatch> availableBatches);
	}
}
