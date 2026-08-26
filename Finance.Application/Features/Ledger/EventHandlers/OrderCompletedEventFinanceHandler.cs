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

public static class OrderCompletedEventFinanceHandler
{
    [Transactional]
    public static async Task Handle(
        OrderCompletedEvent @event,
        IFinanceDataContext context,
        CancellationToken cancellationToken)
    {
        var cashMovement = new CashMovement
        {
            MovementId = Guid.NewGuid(),
            Amount = @event.PaidAmount,
            Type = CashMovementType.Inbound,
            Source = PaymentSource.Sales,
            ReferenceId = @event.OrderId,
            Description = $"Cash collection for Order {@event.OrderId}",
            CreatedAt = @event.CompletedAt
        };

        await context.CashMovements.AddAsync(cashMovement, cancellationToken);

        var journalEntry = new JournalEntry
        {
            JournalEntryId = Guid.NewGuid(),
            ReferenceId = @event.OrderId,
            Description = $"Sales Revenue & COGS for Order {@event.OrderId}",
            CreatedAt = @event.CompletedAt,
            Lines = new List<JournalEntryLine>
            {
                new JournalEntryLine { AccountType = AccountType.Cash, Debit = @event.PaidAmount, Credit = 0 },
                new JournalEntryLine { AccountType = AccountType.SalesRevenue, Debit = 0, Credit = @event.TotalAmount },
                new JournalEntryLine { AccountType = AccountType.COGS, Debit = @event.TotalAmount, Credit = 0 },
                new JournalEntryLine { AccountType = AccountType.InventoryAsset, Debit = 0, Credit = @event.TotalAmount }
            }
        };

        await context.JournalEntries.AddAsync(journalEntry, cancellationToken);
    }
}
