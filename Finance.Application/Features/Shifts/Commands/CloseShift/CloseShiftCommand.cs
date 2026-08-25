namespace Finance.Application.Features.Shifts.Commands.CloseShift;

public sealed record CloseShiftCommand(
    Guid ShiftId,
    decimal ActualCash,
    string? Notes);
