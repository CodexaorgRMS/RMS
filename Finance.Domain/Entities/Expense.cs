using Finance.Domain.Enums;

namespace Finance.Domain.Entities;

public sealed class Expense
{
    public Guid ExpenseId { get; set; } = Guid.NewGuid();
    public Guid CategoryId { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public PaymentSource Source { get; set; }
    public Guid? ShiftId { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public Guid CreatedBy { get; set; }

    public ExpenseCategory? Category { get; set; }
}
