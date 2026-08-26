namespace Finance.Presentation.Requests;

public sealed record OpenShiftRequest(
    Guid CashierId,
    decimal OpeningFloat,
    string? Notes);

public sealed record CloseShiftRequest(
    decimal ActualCash,
    string? Notes);
