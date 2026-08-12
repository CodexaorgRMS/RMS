using Inventory.Application.Abstractions;
using Inventory.Presentation.InventoryItems.Dtos;
using Inventory.Presentation.Shared;

namespace Inventory.Presentation.InventoryItems.Queries;

[ExtendObjectType(typeof(Query))]
public sealed class InventoryItemQueries
{
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<InventoryItemDto> GetInventoryItems(
        [Service] IInventoryDataContext context)
    {
        return context.InventoryItems
            .Select(x => new InventoryItemDto
            {
                InventoryItemId = x.InventoryItemId,
                ProductId = x.ProductId,
                ProductName = x.Product.Name,
                Quantity = x.Quantity,
                MinStock = x.MinStock,
                UpdatedAt = x.UpdatedAt
            });
    }
}