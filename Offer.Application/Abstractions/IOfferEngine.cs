using Offers.Domain.Entities;
using Offers.Domain.Models;

namespace Offers.Application.Abstractions;

public interface IOfferEngine
{
    DiscountEvaluationResult EvaluateOffers(IEnumerable<Offer> activeOffers, List<CartItemInput> cartItems);
}
