using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Inventory.Application.Abstractions;

using Inventory.Presentation.StockMovements.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Presentation.StockMovements.Queries;

[ExtendObjectType(typeof(SharedPresentation.GraphQL.Query))]
public sealed class StockMovementQueries
{
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<StockMovementDto> GetStockMovements(
        [Service] IInventoryDataContext context)
    {
        return context.StockMovements
            .AsNoTracking()
            .Select(x => new StockMovementDto
            {
                MovementId = x.MovementId,
                ProductId = x.ProductId,
                ProductName = x.Product.Name,
                Type = x.Type.ToString(),
                Quantity = x.Quantity,
                ReferenceId = x.ReferenceId,
                CreatedAt = x.CreatedAt
            });
    }

    [UseFirstOrDefault]
    public IQueryable<StockMovementDto> GetStockMovementById(
        Guid movementId,
        [Service] IInventoryDataContext context,
        CancellationToken cancellationToken)
    {
        return  context.StockMovements
            .AsNoTracking()
            .Where(x => x.MovementId == movementId)
            .Select(x => new StockMovementDto
            {
                MovementId = x.MovementId,
                ProductId = x.ProductId,
                ProductName = x.Product.Name,
                Type = x.Type.ToString(),
                Quantity = x.Quantity,
                ReferenceId = x.ReferenceId,
                CreatedAt = x.CreatedAt
            });
    }
}
