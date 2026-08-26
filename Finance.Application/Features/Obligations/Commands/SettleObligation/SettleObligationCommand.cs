using Finance.Domain.Enums;

namespace Finance.Application.Features.Obligations.Commands.SettleObligation;

public sealed record SettleObligationCommand(
    Guid ObligationId,
    decimal Amount,
    PaymentSource Source,
    Guid? ShiftId,
    string? Notes,
    Guid SettledBy);
