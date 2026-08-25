namespace Offers.Presentation.Offers.Requests;

public sealed record CreateFixedOfferRequest(
    string Name,
    string? Description,
    decimal Amount,
    DateTime StartDate,
    DateTime EndDate,
    int Priority = 1,
    List<CreateOfferTargetRequest>? Targets = null);
