namespace Offers.Presentation.Offers.Requests;

public sealed record CreatePercentageOfferRequest(
    string Name,
    string? Description,
    decimal Percentage,
    DateTime StartDate,
    DateTime EndDate,
    int Priority = 1,
    List<CreateOfferTargetRequest>? Targets = null);
