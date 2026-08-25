namespace Finance.Presentation.Requests;

public sealed record CreateObligationRequest(
    string Title,
    int Type,
    int Category,
    decimal TotalAmount,
    DateTime? DueDate,
    string? Notes);

public sealed record SettleObligationRequest(
    decimal Amount,
    int Source,
    Guid? ShiftId,
    string? Notes,
    Guid SettledBy);
