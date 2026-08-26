using Finance.Domain.Enums;

namespace Finance.Domain.Entities;

public sealed class CashMovement
{
    public Guid MovementId { get; set; } = Guid.NewGuid();
    public Guid? ShiftId { get; set; }
    public decimal Amount { get; set; }
    public CashMovementType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public PaymentSource Source { get; set; }
    public Guid? ReferenceId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid CreatedBy { get; set; }
}
