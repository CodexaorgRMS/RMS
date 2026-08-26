using Finance.Domain.Enums;

namespace Finance.Application.Features.CashMovements.Commands.RecordCashMovement;

public sealed record RecordCashMovementCommand(
    Guid? ShiftId,
    decimal Amount,
    CashMovementType Type,
    string Description,
    PaymentSource Source,
    Guid? ReferenceId,
    Guid CreatedBy);
