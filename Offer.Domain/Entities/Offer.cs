using Offers.Domain.Enums;

namespace Offers.Domain.Entities;

public sealed class Offer
{
    public Guid OfferId { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public OfferType Type { get; set; }
    public decimal Value { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    public bool IsSmart { get; set; }
    public int Priority { get; set; } = 1;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<OfferTarget> Targets { get; set; } = new List<OfferTarget>();

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    public void Update(
        string name,
        string? description,
        OfferType type,
        decimal value,
        DateTime startDate,
        DateTime endDate,
        int priority)
    {
        Name = name;
        Description = description;
        Type = type;
        Value = value;
        StartDate = startDate;
        EndDate = endDate;
        Priority = priority;
    }
}
