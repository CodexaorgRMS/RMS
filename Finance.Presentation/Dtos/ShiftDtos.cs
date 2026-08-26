namespace Finance.Presentation.Dtos;

public sealed record ShiftDto
{
    public Guid ShiftId { get; init; }
    public Guid CashierId { get; init; }
    public DateTime OpenedAt { get; init; }
    public DateTime? ClosedAt { get; init; }
    public decimal OpeningFloat { get; init; }
    public decimal? ActualCashEnd { get; init; }
    public decimal? ExpectedCashEnd { get; init; }
    public decimal? Variance { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? Notes { get; init; }
}

public sealed record ShiftSummaryDto
{
    public Guid ShiftId { get; init; }
    public Guid CashierId { get; init; }
    public DateTime OpenedAt { get; init; }
    public DateTime? ClosedAt { get; init; }
    public decimal OpeningFloat { get; init; }
    public decimal? ActualCashEnd { get; init; }
    public decimal? ExpectedCashEnd { get; init; }
    public decimal? Variance { get; init; }
    public string Status { get; init; } = string.Empty;
    public decimal TotalInflows { get; init; }
    public decimal TotalOutflows { get; init; }
    public string? Notes { get; init; }
}
