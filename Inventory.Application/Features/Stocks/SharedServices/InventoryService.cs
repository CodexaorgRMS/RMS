using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Inventory.Interfaces;

namespace Inventory.Application.Features.Stocks.SharedServices
{
	public class InventoryService : IInventoryService
	{
		private readonly IInventoryDataContext _context;

		public InventoryService(IInventoryDataContext context)
		{
			_context = context;
		}
		public async Task<int?> AvailableStock(Guid productId)
		{
			var inventoryItem = await _context.InventoryItems.FirstOrDefaultAsync(x=>x.ProductId==productId);

			if (inventoryItem == null)
				return null;

			return inventoryItem.Quantity;

		}
	}
}
