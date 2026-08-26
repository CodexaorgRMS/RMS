namespace Finance.Presentation.Dtos;

public sealed record CashMovementDto
{
    public Guid MovementId { get; init; }
    public Guid? ShiftId { get; init; }
    public decimal Amount { get; init; }
    public string Type { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string Source { get; init; } = string.Empty;
    public Guid? ReferenceId { get; init; }
    public DateTime CreatedAt { get; init; }
    public Guid CreatedBy { get; init; }
}
