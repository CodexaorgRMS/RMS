using Offers.Domain.Entities;
using Offers.Domain.Enums;
using Offers.Domain.Models;

namespace Offers.Domain.Interfaces;

public interface IOfferStrategy
{
    OfferType Type { get; }
    DiscountEvaluationResult CalculateDiscount(Offer offer, List<CartItemInput> cartItems);
}
