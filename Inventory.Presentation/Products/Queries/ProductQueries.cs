using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Inventory.Application.Abstractions;
using Inventory.Presentation.Products.Dtos;

using Microsoft.EntityFrameworkCore;

namespace Inventory.Presentation.Products.Queries
{
	[ExtendObjectType(typeof(SharedPresentation.GraphQL.Query))]
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

		[UseFirstOrDefault]
		public IQueryable<ProductDto> GetProductById(
			[Service] IInventoryDataContext context,
			Guid productId)
		{
			return  context.Products
				.Where(p => p.ProductId == productId)
				.Select(p => new ProductDto
				{
					ProductId = p.ProductId,
					Name = p.Name,
					Description = p.Description,
					CategoryId = p.CategoryId
				});
		}
	}
}
