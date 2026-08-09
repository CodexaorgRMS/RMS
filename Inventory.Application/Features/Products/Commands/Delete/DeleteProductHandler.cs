using FluentResults;
using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Wolverine.Attributes;

namespace Inventory.Application.Features.Products.Commands.Delete
{
    [Transactional]
    public static class DeleteProductHandler
    {
        public static async Task<Result> Handle(DeleteProductCommand command,
			IInventoryDbContext _context)
        {
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductId == command.ProductId);
            
            if (product != null)
            {
                _context.Products.Remove(product);
                return Result.Ok();
			}

            return Result.Fail($"Product not found.");
		}
    }
}
