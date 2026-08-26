namespace Offers.Presentation.Offers.Requests;

public sealed record CreateBogoOfferRequest(
    string Name,
    string? Description,
    decimal DiscountPercentage,
    DateTime StartDate,
    DateTime EndDate,
    int Priority = 1,
    List<CreateOfferTargetRequest>? Targets = null);
