using FluentResults;
using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Wolverine.Attributes;

namespace Inventory.Application.Features.Products.Commands.Delete
{
    [Transactional]
    public static class DeleteProductHandler
    {
        public static async Task<Result> Handle(
            DeleteProductCommand command,
            IInventoryDataContext context,
            CancellationToken cancellationToken)
        {
            var product = await context.Products
                .FirstAsync(p => p.ProductId == command.ProductId, cancellationToken);

            product.IsActive = false;

            return Result.Ok();
        }
    }
}
