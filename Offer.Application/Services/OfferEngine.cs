using Offers.Application.Abstractions;
using Offers.Domain.Entities;
using Offers.Domain.Enums;
using Offers.Domain.Interfaces;
using Offers.Domain.Models;

namespace Offers.Application.Services;

public sealed class OfferEngine(IEnumerable<IOfferStrategy> strategies) : IOfferEngine
{
    private readonly Dictionary<OfferType, IOfferStrategy> _strategies = strategies.ToDictionary(s => s.Type);

    public DiscountEvaluationResult EvaluateOffers(IEnumerable<Offer> activeOffers, List<CartItemInput> cartItems)
    {
        if (cartItems.Count == 0)
        {
            return DiscountEvaluationResult.Empty;
        }

        var sortedOffers = activeOffers
            .Where(o => o.IsActive)
            .OrderByDescending(o => o.Priority)
            .ThenByDescending(o => o.Value)
            .ToList();

        if (sortedOffers.Count == 0)
        {
            return DiscountEvaluationResult.Empty;
        }

        var allAppliedDetails = new List<AppliedDiscountDetail>();
        decimal totalDiscount = 0m;
        
        var cartSubtotal = cartItems.Sum(i => i.UnitPrice * i.Quantity);

        // Track items and their remaining full-price quantities to prevent invalid stacking
        var remainingCart = cartItems.Select(item => new CartItemInput(
            item.ProductId,
            item.CategoryId,
            item.Quantity,
            item.UnitPrice
        )).ToList();

        foreach (var offer in sortedOffers)
        {
            if (!_strategies.TryGetValue(offer.Type, out var strategy))
            {
                continue;
            }

            var result = strategy.CalculateDiscount(offer, remainingCart);
            if (result.TotalDiscount > 0)
            {
                totalDiscount += result.TotalDiscount;
                allAppliedDetails.AddRange(result.Details);
            }
        }

        totalDiscount = Math.Min(totalDiscount, cartSubtotal);

        return new DiscountEvaluationResult(totalDiscount, allAppliedDetails);
    }
}
