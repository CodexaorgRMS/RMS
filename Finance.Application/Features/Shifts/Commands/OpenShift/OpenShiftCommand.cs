namespace Finance.Application.Features.Shifts.Commands.OpenShift;

public sealed record OpenShiftCommand(
    Guid CashierId,
    decimal OpeningFloat,
    string? Notes);
