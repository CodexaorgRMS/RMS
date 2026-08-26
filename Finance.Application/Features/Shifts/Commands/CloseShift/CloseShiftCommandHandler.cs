using FluentResults;
using Finance.Application.Abstractions;
using Finance.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Finance.Events;
using Wolverine;
using Wolverine.Attributes;

namespace Finance.Application.Features.Shifts.Commands.CloseShift;

[Transactional]
public static class CloseShiftCommandHandler
{
    public static async Task<Result> Handle(
        CloseShiftCommand command,
        IFinanceDataContext context,
        IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var shift = await context.Shifts
            .FirstOrDefaultAsync(s => s.ShiftId == command.ShiftId, cancellationToken);

        if (shift is null)
        {
            return Result.Fail("Shift not found.");
        }

        if (shift.Status == ShiftStatus.Closed)
        {
            return Result.Fail("Shift is already closed.");
        }

        var inflows = await context.CashMovements
            .Where(m => m.ShiftId == shift.ShiftId
                        && m.Source == PaymentSource.Drawer
                        && (m.Type == CashMovementType.OpeningFloat
                            || m.Type == CashMovementType.Deposit
                            || m.Type == CashMovementType.SaleCash
                            || m.Type == CashMovementType.CustomerPayment))
            .SumAsync(m => (decimal?)m.Amount, cancellationToken) ?? 0m;

        var outflows = await context.CashMovements
            .Where(m => m.ShiftId == shift.ShiftId
                        && m.Source == PaymentSource.Drawer
                        && (m.Type == CashMovementType.Withdrawal
                            || m.Type == CashMovementType.ExpensePayment
                            || m.Type == CashMovementType.ObligationSettlement))
            .SumAsync(m => (decimal?)m.Amount, cancellationToken) ?? 0m;

        var expectedCash = inflows - outflows;

        shift.Close(command.ActualCash, expectedCash, command.Notes);

        if (shift.Variance.HasValue && shift.Variance.Value != 0)
        {
            await bus.PublishAsync(new CashShiftClosedWithVarianceIntegrationEvent(
                shift.ShiftId,
                shift.CashierId,
                expectedCash,
                command.ActualCash,
                shift.Variance.Value,
                DateTime.UtcNow));
        }

        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
