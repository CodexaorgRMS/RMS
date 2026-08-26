namespace Offers.Presentation.Offers.Requests;

public sealed record CreateBundleOfferRequest(
    string Name,
    string? Description,
    decimal BundlePrice,
    DateTime StartDate,
    DateTime EndDate,
    int Priority = 1,
    List<CreateOfferTargetRequest>? Targets = null);
