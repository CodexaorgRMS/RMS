using Finance.Application.Abstractions;
using Finance.Domain.Entities;
using Finance.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Customers.Events;
using Wolverine.Attributes;

namespace Finance.Application.EventConsumers;

[Transactional]
public static class CustomerPaymentReceivedConsumer
{
    public static async Task Handle(
        CustomerPaymentReceivedIntegrationEvent @event,
        IFinanceDataContext context,
        CancellationToken cancellationToken)
    {
        if (@event.AmountPaid <= 0)
        {
            return;
        }

        var openShift = await context.Shifts
            .FirstOrDefaultAsync(s => s.CashierId == @event.CashierId && s.Status == ShiftStatus.Open, cancellationToken);

        var source = string.Equals(@event.PaymentMethod, "Bank", StringComparison.OrdinalIgnoreCase)
            ? PaymentSource.Bank
            : PaymentSource.Drawer;

        var cashMovement = new CashMovement
        {
            ShiftId = source == PaymentSource.Drawer ? openShift?.ShiftId : null,
            Amount = @event.AmountPaid,
            Type = CashMovementType.CustomerPayment,
            Description = $"Customer Payment: Customer {@event.CustomerId} (Method: {@event.PaymentMethod})",
            Source = source,
            ReferenceId = @event.CustomerId,
            CreatedBy = @event.CashierId,
            CreatedAt = @event.OccurredAt
        };

        context.CashMovements.Add(cashMovement);
        await context.SaveChangesAsync(cancellationToken);
    }
}
