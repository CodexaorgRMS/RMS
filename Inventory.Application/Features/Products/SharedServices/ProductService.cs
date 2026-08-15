using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Inventory.Dtos;
using SharedContracts.Inventory.Interfaces;

namespace Inventory.Application.Features.Products.SharedServices
{
	public class ProductService : IProductService
	{
		private readonly IInventoryDataContext _context;

		public ProductService(IInventoryDataContext context)
		{
			_context = context;
		}

		public async Task<ProductDto?> GetProductByIdAsync(Guid productId)
		{
			var product =await _context.Products.FirstOrDefaultAsync(p => p.ProductId == productId);

			if (product == null)
				return null;

			return new ProductDto(
		
				Name : product.Name,
				Description : product.Description,
				SellingPrice : product.SellingPrice,
				CategoryId : product.CategoryId,
				IsActive : product.IsActive,
				Barcode : product.Barcode
			);

		}

		public async Task<bool> ProductExistsAsync(Guid productId)
		{
			return await _context.Products.AnyAsync(p => p.ProductId == productId);
		}
	}
}
