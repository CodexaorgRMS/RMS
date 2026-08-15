using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Inventory.Application.Abstractions;
using Inventory.Domain.Entities;
using Inventory.Presentation.Shared;
using Microsoft.EntityFrameworkCore;
using BatchStatus = Inventory.Domain.Enums.BatchStatus;

namespace Inventory.Presentation.ProductBatches.Queries;

[ExtendObjectType(typeof(Query))]
public class ProductBatchQueries
{
    // Retrieve all batches with pagination, filtering, projection, and sorting
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<ProductBatch> GetProductBatches(
        [Service] IInventoryDataContext context)
    {
        return context.ProductBatches.AsNoTracking();
    }

    // Retrieve only quarantined batches (OnHold or Recalled) for warehouse audits & supplier returns
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<ProductBatch> GetQuarantinedBatches(
        [Service] IInventoryDataContext context)
    {
        return context.ProductBatches
            .AsNoTracking()
            .Where(b => b.Status == BatchStatus.OnHold || b.Status == BatchStatus.Recalled);
    }

    // Retrieve a specific batch by its ID
    [UseFirstOrDefault]
    [UseProjection]
    public IQueryable<ProductBatch> GetProductBatchById(
        [Service] IInventoryDataContext context,
        Guid batchId)
    {
        return context.ProductBatches
            .AsNoTracking()
            .Where(b => b.BatchId == batchId);
    }

    // Retrieve batches nearing expiration date based on threshold
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<ProductBatch> GetExpiringBatches(
        [Service] IInventoryDataContext context,
        int withinDays = 7)
    {
        var targetDate = DateTime.UtcNow.AddDays(withinDays);

        return context.ProductBatches
            .AsNoTracking()
            .Where(b => b.Status == BatchStatus.Active &&
                        b.CurrentQuantity > 0 &&
                        b.ExpiryDate <= targetDate &&
                        b.ExpiryDate > DateTime.UtcNow);
    }
}