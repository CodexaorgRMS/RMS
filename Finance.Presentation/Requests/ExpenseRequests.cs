namespace Finance.Presentation.Requests;

public sealed record CreateExpenseCategoryRequest(
    string Name,
    string? Description);

public sealed record RecordExpenseRequest(
    Guid CategoryId,
    decimal Amount,
    string Description,
    int Source,
    Guid? ShiftId,
    Guid CreatedBy);
