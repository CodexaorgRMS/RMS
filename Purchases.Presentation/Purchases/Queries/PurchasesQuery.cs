using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Purchases.Application.Abstractions;
using Purchases.Domain.Entities;
using SharedPresentation.GraphQL;

namespace Purchases.Presentation.Purchases.Queries;

[ExtendObjectType(typeof(Query))]
public class PurchaseQueries
{
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<PurchaseOrder> GetPurchases(
        [Service] IPurchasesDataContext context)
    {
        return context.PurchaseOrders
            .AsNoTracking();
    }


    [UseFirstOrDefault]
    public IQueryable<PurchaseOrder> GetPurchaseById(
        Guid purchaseId,
        [Service] IPurchasesDataContext context)
    {
        return context.PurchaseOrders
            .Where(x => x.PurchaseOrderId == purchaseId)
            .AsNoTracking();
    }
}