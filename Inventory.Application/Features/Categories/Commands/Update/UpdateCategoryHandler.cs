using FluentResults;
using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Wolverine.Attributes;

namespace Inventory.Application.Features.Categories.Commands.Update
{
    [Transactional]
    public static class UpdateCategoryHandler
    {
        public static async Task<Result> Handle(
            UpdateCategoryCommand command,
            IInventoryDataContext context,
            CancellationToken cancellationToken)
        {
            var category = await context.Categories
                .FirstAsync(c => c.CategoryId == command.CategoryId, cancellationToken);

            category.Name = command.Name;
            category.Description = command.Description;
            category.ParentId = command.ParentId;

            return Result.Ok();
        }
    }
}
