using Finance.Application.Abstractions;
using Finance.Domain.Entities;
using Finance.Domain.Enums;
using SharedContracts.Sales.Events;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Finance.Application.Features.Ledger.EventHandlers;

public static class OrderCompletedEventFinanceHandler
{
    public static async Task Handle(
        OrderCompletedEvent @event,
        IFinanceDataContext context,
        CancellationToken cancellationToken)
    {
        var alreadyProcessed = await context.JournalEntries
            .AnyAsync(j => j.ReferenceId == @event.OrderId, cancellationToken);

        if (alreadyProcessed)
        {
            return;
        }

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

        decimal debt = @event.TotalAmount - @event.PaidAmount;
        var lines = new List<JournalEntryLine>
        {
            new JournalEntryLine { AccountType = AccountType.Cash, Debit = @event.PaidAmount, Credit = 0 },
            new JournalEntryLine { AccountType = AccountType.SalesRevenue, Debit = 0, Credit = @event.TotalAmount },
            new JournalEntryLine { AccountType = AccountType.COGS, Debit = @event.TotalAmount, Credit = 0 },
            new JournalEntryLine { AccountType = AccountType.InventoryAsset, Debit = 0, Credit = @event.TotalAmount }
        };

        if (debt > 0)
        {
            lines.Add(new JournalEntryLine { AccountType = AccountType.AccountsReceivable, Debit = debt, Credit = 0 });
        }

        var journalEntry = new JournalEntry
        {
            JournalEntryId = Guid.NewGuid(),
            ReferenceId = @event.OrderId,
            Description = $"Sales Revenue & COGS for Order {@event.OrderId}",
            CreatedAt = @event.CompletedAt,
            Lines = lines
        };

        await context.JournalEntries.AddAsync(journalEntry, cancellationToken);
        try
        {
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"DB UPDATE EXCEPTION! Inner: {ex.InnerException?.Message}");
            throw;
        }
    }
}
