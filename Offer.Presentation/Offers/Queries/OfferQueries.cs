using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Offers.Application.Abstractions;
using Offers.Domain.Entities;
using Offers.Domain.Models;
using Offers.Infrastructure.Data;
using SharedPresentation.GraphQL;

namespace Offers.Presentation.Offers.Queries;

[ExtendObjectType(typeof(Query))]
public class OfferQueries
{
    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Offer> GetOffers([Service] OffersDbContext context)
    {
        return context.Offers
            .AsNoTracking()
            .Include(o => o.Targets);
    }

    [UseFirstOrDefault]
    [UseProjection]
    public IQueryable<Offer> GetOfferById(
        Guid offerId,
        [Service] OffersDbContext context)
    {
        return context.Offers
            .AsNoTracking()
            .Include(o => o.Targets)
            .Where(o => o.OfferId == offerId);
    }

    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Offer> GetActiveOffers([Service] OffersDbContext context)
    {
        var now = DateTime.UtcNow;
        return context.Offers
            .AsNoTracking()
            .Include(o => o.Targets)
            .Where(o => o.IsActive && o.StartDate <= now && o.EndDate >= now);
    }

    [UsePaging(IncludeTotalCount = true)]
    [UseProjection]
    [UseFiltering]
    [UseSorting]
    public IQueryable<Offer> GetSmartExpiryOffers([Service] OffersDbContext context)
    {
        return context.Offers
            .AsNoTracking()
            .Include(o => o.Targets)
            .Where(o => o.IsSmart);
    }

    public async Task<DiscountEvaluationResult> EvaluateCartAsync(
        List<CartItemInput> items,
        [Service] IOfferEngine engine,
        [Service] OffersDbContext context,
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;

        var activeOffers = await context.Offers
            .AsNoTracking()
            .Include(o => o.Targets)
            .Where(o => o.IsActive && o.StartDate <= now && o.EndDate >= now)
            .ToListAsync(cancellationToken);

        return engine.EvaluateOffers(activeOffers, items);
    }
}
