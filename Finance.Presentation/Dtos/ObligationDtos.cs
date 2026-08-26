namespace Finance.Presentation.Dtos;

public sealed record ObligationSettlementDto
{
    public Guid SettlementId { get; init; }
    public Guid ObligationId { get; init; }
    public decimal AmountPaid { get; init; }
    public string Source { get; init; } = string.Empty;
    public Guid? ShiftId { get; init; }
    public string? Notes { get; init; }
    public DateTime SettledAt { get; init; }
    public Guid SettledBy { get; init; }
}

public sealed record FinancialObligationDto
{
    public Guid ObligationId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Type { get; init; } = string.Empty;
    public string Category { get; init; } = string.Empty;
    public decimal TotalAmount { get; init; }
    public decimal PaidAmount { get; init; }
    public decimal RemainingAmount { get; init; }
    public DateTime? DueDate { get; init; }
    public string Status { get; init; } = string.Empty;
    public string? Notes { get; init; }
    public DateTime CreatedAt { get; init; }
    public IReadOnlyList<ObligationSettlementDto> Settlements { get; init; } = [];
}
