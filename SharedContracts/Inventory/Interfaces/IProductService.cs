using SharedContracts.Inventory.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace SharedContracts.Inventory.Interfaces
{
	public interface IProductService
	{
		Task<bool> ProductExistsAsync(Guid productId);
		Task<ProductDto?> GetProductByIdAsync(Guid productId);
	}
}
