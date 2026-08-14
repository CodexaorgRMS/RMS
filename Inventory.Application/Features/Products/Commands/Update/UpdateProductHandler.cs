using FluentResults;
using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Wolverine.Attributes;

namespace Inventory.Application.Features.Products.Commands.Update
{
    [Transactional]
    public static class UpdateProductHandler
    {
        public static async Task<Result> Handle(
            UpdateProductCommand command,
            IInventoryDataContext context,
            CancellationToken cancellationToken)
        {
            var product = await context.Products
                .FirstAsync(p => p.ProductId == command.ProductId, cancellationToken);

            product.Name = command.Name;
            product.Description = command.Description;
            product.CategoryId = command.CategoryId;

            return Result.Ok();
        }
    }
}
