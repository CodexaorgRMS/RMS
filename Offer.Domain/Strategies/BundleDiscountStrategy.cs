using Offers.Domain.Entities;
using Offers.Domain.Enums;
using Offers.Domain.Interfaces;
using Offers.Domain.Models;

namespace Offers.Domain.Strategies;

public sealed class BundleDiscountStrategy : IOfferStrategy
{
    public OfferType Type => OfferType.Bundle;

    public DiscountEvaluationResult CalculateDiscount(Offer offer, List<CartItemInput> cartItems)
    {
        if (offer.Targets.Count == 0 || cartItems.Count == 0)
        {
            return DiscountEvaluationResult.Empty;
        }

        // Calculate available bundle sets
        int maxPossibleSets = int.MaxValue;
        var targetCartItemMatches = new Dictionary<Guid, (OfferTarget Target, List<CartItemInput> MatchingItems, int AvailableQuantity)>();

        foreach (var target in offer.Targets)
        {
            var matchingItems = cartItems.Where(i =>
                (target.TargetType == OfferTargetType.Product && i.ProductId == target.TargetId) ||
                (target.TargetType == OfferTargetType.Category && i.CategoryId == target.TargetId)
            ).ToList();

            int availableQty = matchingItems.Sum(i => i.Quantity);
            int requiredQty = Math.Max(1, target.RequiredQuantity);
            int possibleSetsForTarget = availableQty / requiredQty;

            if (possibleSetsForTarget < maxPossibleSets)
            {
                maxPossibleSets = possibleSetsForTarget;
            }

            targetCartItemMatches[target.OfferTargetId] = (target, matchingItems, availableQty);
        }

        if (maxPossibleSets == 0 || maxPossibleSets == int.MaxValue)
        {
            return DiscountEvaluationResult.Empty;
        }

        // Calculate standard price per bundle vs discounted bundle price
        decimal standardPricePerBundle = 0m;
        decimal bundlePricePerBundle = 0m;
        bool hasSpecialTargetPricing = offer.Targets.Any(t => t.SpecialPrice.HasValue);

        foreach (var target in offer.Targets)
        {
            var matching = targetCartItemMatches[target.OfferTargetId];
            decimal avgUnitPrice = matching.MatchingItems.Count > 0
                ? matching.MatchingItems.Average(i => i.UnitPrice)
                : 0m;

            int requiredQty = Math.Max(1, target.RequiredQuantity);
            standardPricePerBundle += avgUnitPrice * requiredQty;

            if (target.SpecialPrice.HasValue)
            {
                bundlePricePerBundle += target.SpecialPrice.Value * requiredQty;
            }
            else
            {
                bundlePricePerBundle += avgUnitPrice * requiredQty;
            }
        }

        // If offer has an overall Value configured (e.g. Bundle for $50), use that as the bundle set price
        if (offer.Value > 0 && !hasSpecialTargetPricing)
        {
            bundlePricePerBundle = offer.Value;
        }

        decimal discountPerSet = standardPricePerBundle - bundlePricePerBundle;
        if (discountPerSet <= 0)
        {
            return DiscountEvaluationResult.Empty;
        }

        decimal totalDiscount = Math.Round(maxPossibleSets * discountPerSet, 2, MidpointRounding.AwayFromZero);
        var details = new List<AppliedDiscountDetail>
        {
            new(
                offer.OfferId,
                offer.Name,
                totalDiscount,
                $"Bundle deal applied for {maxPossibleSets} set(s). Saved ${discountPerSet} per set."
            )
        };

        return new DiscountEvaluationResult(totalDiscount, details);
    }
}
