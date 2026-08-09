using FluentResults;
using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Wolverine.Attributes;

namespace Inventory.Application.Features.Products.Commands.Update
{
    [Transactional]
    public static class UpdateProductHandler
    {

        public static async Task<Result> Handle(UpdateProductCommand command,
			IInventoryDbContext _context)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == command.ProductId);
            
            if (product == null)
                return Result.Fail("Product not found");

            product.Name = command.Name;
            product.Description = command.Description;
            product.CategoryId = command.CategoryId;

            return Result.Ok();

		}
    }
}
