using System;
using System.Collections.Generic;
using System.Text;

namespace SharedContracts.Inventory.Interfaces
{
	public interface IInventoryService
	{
		Task<int?> AvailableStock(Guid productId);
	}
}
