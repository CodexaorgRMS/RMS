using Finance.Domain.Enums;

namespace Finance.Domain.Entities;

public sealed class ObligationSettlement
{
    public Guid SettlementId { get; set; } = Guid.NewGuid();
    public Guid ObligationId { get; set; }
    public decimal AmountPaid { get; set; }
    public PaymentSource Source { get; set; }
    public Guid? ShiftId { get; set; }
    public string? Notes { get; set; }
    public DateTime SettledAt { get; set; } = DateTime.UtcNow;
    public Guid SettledBy { get; set; }

    public FinancialObligation? Obligation { get; set; }
}
