using FluentResults;
using Inventory.Application.Abstractions;
using Inventory.Domain.Entities;
using Wolverine.Attributes;

namespace Inventory.Application.Features.Products.Commands.Create
{

    [Transactional]
    public static class CreateProductHandler
    {
        public static async Task< Result< Guid>> Handle(CreateProductCommand command,
			IInventoryDataContext _context)
        {
            var product = new Product
            {
                ProductId = Guid.NewGuid(),
                Name = command.Name,
                Description = command.Description,
                CategoryId = command.CategoryId
            };

           await _context.Products.AddAsync(product);

            return Result.Ok( product.ProductId);
        }
    }
}
