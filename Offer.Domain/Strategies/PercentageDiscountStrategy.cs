using Offers.Domain.Entities;
using Offers.Domain.Enums;
using Offers.Domain.Interfaces;
using Offers.Domain.Models;

namespace Offers.Domain.Strategies;

public sealed class PercentageDiscountStrategy : IOfferStrategy
{
    public OfferType Type => OfferType.Percentage;

    public DiscountEvaluationResult CalculateDiscount(Offer offer, List<CartItemInput> cartItems)
    {
        if (offer.Value <= 0 || cartItems.Count == 0)
        {
            return DiscountEvaluationResult.Empty;
        }

        var details = new List<AppliedDiscountDetail>();
        decimal totalDiscount = 0m;

        var targetProductIds = offer.Targets
            .Where(t => t.TargetType == OfferTargetType.Product)
            .Select(t => t.TargetId)
            .ToHashSet();

        var targetCategoryIds = offer.Targets
            .Where(t => t.TargetType == OfferTargetType.Category)
            .Select(t => t.TargetId)
            .ToHashSet();

        bool hasTargets = targetProductIds.Count > 0 || targetCategoryIds.Count > 0;

        foreach (var item in cartItems)
        {
            bool isEligible = !hasTargets ||
                              targetProductIds.Contains(item.ProductId) ||
                              targetCategoryIds.Contains(item.CategoryId);

            if (!isEligible)
            {
                continue;
            }

            decimal itemTotal = item.Quantity * item.UnitPrice;
            decimal discount = Math.Round(itemTotal * (offer.Value / 100m), 2, MidpointRounding.AwayFromZero);

            if (discount > 0)
            {
                totalDiscount += discount;
                details.Add(new AppliedDiscountDetail(
                    offer.OfferId,
                    offer.Name,
                    discount,
                    $"{offer.Value}% off product {item.ProductId} (Qty: {item.Quantity})"
                ));
            }
        }

        return new DiscountEvaluationResult(totalDiscount, details);
    }
}
