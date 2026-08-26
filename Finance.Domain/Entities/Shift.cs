using Finance.Domain.Enums;

namespace Finance.Domain.Entities;

public sealed class Shift
{
    public Guid ShiftId { get; set; } = Guid.NewGuid();
    public Guid CashierId { get; set; }
    public DateTime OpenedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ClosedAt { get; set; }
    public decimal OpeningFloat { get; set; }
    public decimal? ActualCashEnd { get; set; }
    public decimal? ExpectedCashEnd { get; set; }
    public decimal? Variance { get; set; }
    public ShiftStatus Status { get; set; } = ShiftStatus.Open;
    public string? Notes { get; set; }

    public void Close(decimal actualCash, decimal expectedCash, string? notes)
    {
        if (Status == ShiftStatus.Closed)
        {
            throw new InvalidOperationException("Shift is already closed.");
        }

        ActualCashEnd = actualCash;
        ExpectedCashEnd = expectedCash;
        Variance = actualCash - expectedCash;
        ClosedAt = DateTime.UtcNow;
        Status = ShiftStatus.Closed;

        if (!string.IsNullOrWhiteSpace(notes))
        {
            Notes = string.IsNullOrWhiteSpace(Notes) ? notes : $"{Notes} | {notes}";
        }
    }
}
