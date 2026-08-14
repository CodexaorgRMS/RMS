using FluentResults;
using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Wolverine.Attributes;

namespace Inventory.Application.Features.Categories.Commands.Delete
{
    [Transactional]
    public static class DeleteCategoryHandler
    {
        public static async Task<Result> Handle(
            DeleteCategoryCommand command,
            IInventoryDataContext context,
            CancellationToken cancellationToken)
        {
            var category = await context.Categories
                .FirstAsync(c => c.CategoryId == command.CategoryId, cancellationToken);

            context.Categories.Remove(category);

            return Result.Ok();
        }
    }
}
