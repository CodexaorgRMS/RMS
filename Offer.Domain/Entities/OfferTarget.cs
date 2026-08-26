using Offers.Domain.Enums;

namespace Offers.Domain.Entities;

public sealed class OfferTarget
{
    public Guid OfferTargetId { get; set; } = Guid.NewGuid();
    public Guid OfferId { get; set; }
    public OfferTargetType TargetType { get; set; }
    public Guid TargetId { get; set; }
    public int RequiredQuantity { get; set; } = 1;
    public decimal? SpecialPrice { get; set; }
}
