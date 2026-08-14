using FluentResults;
using Inventory.Application.Abstractions;
using Inventory.Application.Features.Categories.Commands.Create;
using Inventory.Domain.Entities;
using System;
using System.Threading.Tasks;
using Wolverine.Attributes;

namespace Inventory.Application.Features.Categories.Commands.Create
{
    [Transactional]
    public static class CreateCategoryHandler
    {
        public static async Task<Result<Guid>> Handle(
            CreateCategoryCommand command,
            IInventoryDataContext context,
            CancellationToken cancellationToken)
        {
            var category = new Category
            {
                Name = command.Name,
                Description = command.Description,
                ParentId = command.ParentId
            };

            await context.Categories.AddAsync(category, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return Result.Ok(category.CategoryId);
        }
    }
}
