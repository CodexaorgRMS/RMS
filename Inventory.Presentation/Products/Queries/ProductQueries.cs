using Inventory.Application.Abstractions;
using Inventory.Presentation.Products.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Presentation.Products.Queries
{
	public class ProductQueries
	{
		[UsePaging(IncludeTotalCount = true)]
		[UseFiltering]
		[UseSorting]
		public IQueryable<ProductDto> GetProducts(
			[Service] IInventoryDataContext context)
		{
			return context.Products.Select(p => new ProductDto
			{
				ProductId = p.ProductId,
				Name = p.Name,
				Description = p.Description,
				CategoryId = p.CategoryId
			});
		}

		public async Task< ProductDto?> GetProductById(
			[Service] IInventoryDataContext context,
			Guid productId)
		{
			return await context.Products
				.Where(p => p.ProductId == productId)
				.Select(p => new ProductDto
				{
					ProductId = p.ProductId,
					Name = p.Name,
					Description = p.Description,
					CategoryId = p.CategoryId
				})
				.FirstOrDefaultAsync();
		}
	}
}
