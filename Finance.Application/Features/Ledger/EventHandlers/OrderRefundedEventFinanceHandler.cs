using Finance.Application.Abstractions;
using Finance.Domain.Entities;
using Finance.Domain.Enums;
using SharedContracts.Sales.Events;
using Wolverine.Attributes;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Finance.Application.Features.Ledger.EventHandlers;

public static class OrderRefundedEventFinanceHandler
{
    [Transactional]
    public static async Task Handle(
        OrderRefundedEvent @event,
        IFinanceDataContext context,
        CancellationToken cancellationToken)
    {
        var cashMovement = new CashMovement
        {
            MovementId = Guid.NewGuid(),
            Amount = @event.PaidAmount,
            Type = CashMovementType.Outbound,
            Source = PaymentSource.Refund,
            ReferenceId = @event.OrderId,
            Description = $"Refund for Order {@event.OrderId}",
            CreatedAt = @event.RefundedAt
        };

        await context.CashMovements.AddAsync(cashMovement, cancellationToken);

        var journalEntry = new JournalEntry
        {
            JournalEntryId = Guid.NewGuid(),
            ReferenceId = @event.OrderId,
            Description = $"Refund reversal for Order {@event.OrderId}",
            CreatedAt = @event.RefundedAt,
            Lines = new List<JournalEntryLine>
            {
                new JournalEntryLine { AccountType = AccountType.SalesRevenue, Debit = @event.TotalAmount, Credit = 0 },
                new JournalEntryLine { AccountType = AccountType.Cash, Debit = 0, Credit = @event.PaidAmount },
                new JournalEntryLine { AccountType = AccountType.InventoryAsset, Debit = @event.TotalAmount, Credit = 0 },
                new JournalEntryLine { AccountType = AccountType.COGS, Debit = 0, Credit = @event.TotalAmount }
            }
        };

        await context.JournalEntries.AddAsync(journalEntry, cancellationToken);
    }
}
