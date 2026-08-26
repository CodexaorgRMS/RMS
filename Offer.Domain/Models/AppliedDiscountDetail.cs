namespace Offers.Domain.Models;

public sealed record AppliedDiscountDetail(
    Guid OfferId,
    string OfferName,
    decimal DiscountAmount,
    string Description);
