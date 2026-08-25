using Finance.Domain.Enums;

namespace Finance.Domain.Entities;

public sealed class FinancialObligation
{
    public Guid ObligationId { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public ObligationType Type { get; set; }
    public ObligationCategory Category { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal PaidAmount { get; set; }
    public decimal RemainingAmount { get; set; }
    public DateTime? DueDate { get; set; }
    public ObligationStatus Status { get; set; } = ObligationStatus.Pending;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<ObligationSettlement> Settlements { get; set; } = new List<ObligationSettlement>();

    public void Settle(decimal amount)
    {
        if (Status == ObligationStatus.Cancelled)
        {
            throw new InvalidOperationException("Cannot settle a cancelled obligation.");
        }

        if (Status == ObligationStatus.FullySettled)
        {
            throw new InvalidOperationException("Obligation is already fully settled.");
        }

        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Settlement amount must be greater than zero.");
        }

        if (amount > RemainingAmount)
        {
            throw new InvalidOperationException($"Settlement amount ({amount}) exceeds remaining balance ({RemainingAmount}).");
        }

        PaidAmount += amount;
        RemainingAmount = TotalAmount - PaidAmount;
        Status = RemainingAmount <= 0 ? ObligationStatus.FullySettled : ObligationStatus.PartiallySettled;
    }

    public void Cancel()
    {
        if (Status == ObligationStatus.FullySettled)
        {
            throw new InvalidOperationException("Cannot cancel a fully settled obligation.");
        }

        Status = ObligationStatus.Cancelled;
    }
}
