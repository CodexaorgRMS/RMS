using Inventory.Application.Abstractions;
using Inventory.Presentation.Shared;
using Inventory.Presentation.StockMovements.Dtos;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Presentation.StockMovements.Queries;

[ExtendObjectType(typeof(Query))]
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
                Type = x.Type,
                Quantity = x.Quantity,
                ReferenceId = x.ReferenceId,
                CreatedAt = x.CreatedAt
            });
    }

    public async Task<StockMovementDto?> GetStockMovementById(
        Guid movementId,
        [Service] IInventoryDataContext context,
        CancellationToken cancellationToken)
    {
        return await context.StockMovements
            .AsNoTracking()
            .Where(x => x.MovementId == movementId)
            .Select(x => new StockMovementDto
            {
                MovementId = x.MovementId,
                ProductId = x.ProductId,
                ProductName = x.Product.Name,
                Type = x.Type,
                Quantity = x.Quantity,
                ReferenceId = x.ReferenceId,
                CreatedAt = x.CreatedAt
            })
            .FirstOrDefaultAsync(cancellationToken);
    }
}