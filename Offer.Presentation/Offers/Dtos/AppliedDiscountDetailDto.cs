namespace Offers.Presentation.Offers.Dtos;

public sealed record AppliedDiscountDetailDto(
    Guid OfferId,
    string OfferName,
    decimal DiscountAmount,
    string Description);
