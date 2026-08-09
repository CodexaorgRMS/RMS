using Inventory.Application.Abstractions;
using Inventory.Application.Features.Categories.Commands.Delete;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using FluentResults;
using Wolverine.Attributes;

namespace Inventory.Application.Features.Categories.Commands.Delete
{
    [Transactional]
    public static class DeleteCategoryHandler
    { 

        public static async Task<Result> Handle(DeleteCategoryCommand command, IInventoryDbContext _context)
        {
            var category = await _context.Categories
                .Include(c => c.Children)
                .Include(c => c.Products)
                .FirstOrDefaultAsync(c => c.CategoryId == command.CategoryId);
            
            if (category == null)
            {
                return Result.Fail("Category not found");
            }

            if (category.Children.Any())
            {
                return Result.Fail("Cannot delete category because it has child categories.");
            }

            if (category.Products.Any())
            {
                return Result.Fail("Cannot delete category because it contains products.");
            }

            _context.Categories.Remove(category);

            return Result.Ok();
		}
    }
}
