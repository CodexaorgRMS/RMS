using Finance.Domain.Enums;

namespace Finance.Application.Features.Expenses.Commands.RecordExpense;

public sealed record RecordExpenseCommand(
    Guid CategoryId,
    decimal Amount,
    string Description,
    PaymentSource Source,
    Guid? ShiftId,
    Guid CreatedBy);
