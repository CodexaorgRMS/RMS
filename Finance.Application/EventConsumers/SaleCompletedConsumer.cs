using Finance.Application.Abstractions;
using Finance.Domain.Entities;
using Finance.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Sales.Events;
using Wolverine.Attributes;

namespace Finance.Application.EventConsumers;

[Transactional]
public static class SaleCompletedConsumer
{
    public static async Task Handle(
        SaleCompletedIntegrationEvent @event,
        IFinanceDataContext context,
        CancellationToken cancellationToken)
    {
        if (@event.CashAmount <= 0)
        {
            return;
        }

        var openShift = await context.Shifts
            .FirstOrDefaultAsync(s => s.CashierId == @event.CashierId && s.Status == ShiftStatus.Open, cancellationToken);

        var cashMovement = new CashMovement
        {
            ShiftId = openShift?.ShiftId,
            Amount = @event.CashAmount,
            Type = CashMovementType.SaleCash,
            Description = $"Sale Cash Receipt: Sale {@event.SaleId}",
            Source = PaymentSource.Drawer,
            ReferenceId = @event.SaleId,
            CreatedBy = @event.CashierId,
            CreatedAt = @event.OccurredAt
        };

        context.CashMovements.Add(cashMovement);
        await context.SaveChangesAsync(cancellationToken);
    }
}
