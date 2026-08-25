using Offers.Domain.Entities;
using Offers.Domain.Enums;
using Offers.Domain.Interfaces;
using Offers.Domain.Models;

namespace Offers.Domain.Strategies;

public sealed class BogoDiscountStrategy : IOfferStrategy
{
    public OfferType Type => OfferType.Bogo;

    public DiscountEvaluationResult CalculateDiscount(Offer offer, List<CartItemInput> cartItems)
    {
        if (cartItems.Count == 0)
        {
            return DiscountEvaluationResult.Empty;
        }

        var targetProductIds = offer.Targets
            .Where(t => t.TargetType == OfferTargetType.Product)
            .Select(t => t.TargetId)
            .ToHashSet();

        var targetCategoryIds = offer.Targets
            .Where(t => t.TargetType == OfferTargetType.Category)
            .Select(t => t.TargetId)
            .ToHashSet();

        bool hasTargets = targetProductIds.Count > 0 || targetCategoryIds.Count > 0;

        // Flatten all eligible items into individual units with their unit prices
        var eligibleUnits = new List<(Guid ProductId, decimal UnitPrice)>();

        foreach (var item in cartItems)
        {
            bool isEligible = !hasTargets ||
                              targetProductIds.Contains(item.ProductId) ||
                              targetCategoryIds.Contains(item.CategoryId);

            if (isEligible && item.Quantity > 0)
            {
                for (int i = 0; i < item.Quantity; i++)
                {
                    eligibleUnits.Add((item.ProductId, item.UnitPrice));
                }
            }
        }

        if (eligibleUnits.Count == 0)
        {
            return DiscountEvaluationResult.Empty;
        }

        // Required buy quantity (default Buy 1 Get 1 free: requiredBuy = 1, groupSize = 2)
        int requiredBuy = offer.Targets.FirstOrDefault()?.RequiredQuantity ?? 1;
        if (requiredBuy < 1) requiredBuy = 1;
        int groupSize = requiredBuy + 1;

        int freeUnitsCount = eligibleUnits.Count / groupSize;
        if (freeUnitsCount == 0)
        {
            return DiscountEvaluationResult.Empty;
        }

        // Sort items ascending by price - cheapest units get discounted
        var sortedUnits = eligibleUnits.OrderBy(u => u.UnitPrice).ToList();
        var discountedUnits = sortedUnits.Take(freeUnitsCount).ToList();

        decimal discountPercentage = offer.Value > 0 ? offer.Value : 100m;
        decimal totalDiscount = 0m;
        var details = new List<AppliedDiscountDetail>();

        foreach (var unit in discountedUnits)
        {
            decimal unitDiscount = Math.Round(unit.UnitPrice * (discountPercentage / 100m), 2, MidpointRounding.AwayFromZero);
            totalDiscount += unitDiscount;

            details.Add(new AppliedDiscountDetail(
                offer.OfferId,
                offer.Name,
                unitDiscount,
                $"BOGO (Buy {requiredBuy} Get 1 at {discountPercentage}% off): discounted product {unit.ProductId} by ${unitDiscount}"
            ));
        }

        return new DiscountEvaluationResult(totalDiscount, details);
    }
}
