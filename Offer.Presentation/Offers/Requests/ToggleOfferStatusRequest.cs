namespace Offers.Presentation.Offers.Requests;

public sealed record ToggleOfferStatusRequest(bool? IsActive = null);
