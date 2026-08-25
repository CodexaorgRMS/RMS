namespace Finance.Presentation.Requests;

public sealed record RecordCashMovementRequest(
    Guid? ShiftId,
    decimal Amount,
    int Type,
    string Description,
    int Source,
    Guid? ReferenceId,
    Guid CreatedBy);
