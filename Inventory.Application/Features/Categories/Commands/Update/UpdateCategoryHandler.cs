using Inventory.Application.Abstractions;
using Inventory.Application.Features.Categories.Commands.Update;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using FluentResults;
using Wolverine.Attributes;

namespace Inventory.Application.Features.Categories.Commands.Update
{
    [Transactional]
    public static class UpdateCategoryHandler
    {

        public static async Task<Result> Handle(UpdateCategoryCommand command, IInventoryDataContext _context)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryId == command.CategoryId);
            
            if (category == null)
                return Result.Fail("Category not found");

            category.Name = command.Name;
            category.Description = command.Description;
            category.ParentId = command.ParentId;

            return Result.Ok();

		}
    }
}
