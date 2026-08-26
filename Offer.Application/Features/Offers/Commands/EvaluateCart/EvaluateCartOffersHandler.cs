using FluentResults;
using Microsoft.EntityFrameworkCore;
using Offers.Application.Abstractions;
using Offers.Domain.Models;

namespace Offers.Application.Features.Offers.Commands.EvaluateCart;

public static class EvaluateCartOffersHandler
{
    public static async Task<Result<DiscountEvaluationResult>> Handle(
        EvaluateCartOffersCommand command,
        IOffersDataContext context,
        IOfferEngine offerEngine,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;

        var activeOffers = await context.Offers
            .Include(o => o.Targets)
            .Where(o => o.IsActive && o.StartDate <= now && o.EndDate >= now)
            .ToListAsync(cancellationToken);

        var evaluation = offerEngine.EvaluateOffers(activeOffers, command.Items);

        return Result.Ok(evaluation);
    }
}
