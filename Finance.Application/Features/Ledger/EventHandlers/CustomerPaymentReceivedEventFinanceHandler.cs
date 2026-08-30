using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Finance.Application.Abstractions;
using Finance.Domain.Entities;
using Finance.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Customers.Events;
using Wolverine.Attributes;

namespace Finance.Application.Features.Ledger.EventHandlers;

public static class CustomerPaymentReceivedEventFinanceHandler
{
    [Transactional]
    public static async Task Handle(
        CustomerPaymentReceivedEvent @event,
        IFinanceDataContext context,
        CancellationToken cancellationToken)
    {
        // Issue 4 & 8: Idempotency check for missing AR cash integration
        var alreadyProcessed = await context.JournalEntries
            .AnyAsync(j => j.ReferenceId == @event.CustomerId && j.Description.Contains("Customer Payment"), cancellationToken);

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
            ReferenceId = @event.CustomerId,
            Description = $"Cash collection from Customer {@event.CustomerId}",
            CreatedAt = @event.PaymentDate
        };

        await context.CashMovements.AddAsync(cashMovement, cancellationToken);

        var journalEntry = new JournalEntry
        {
            JournalEntryId = Guid.NewGuid(),
            ReferenceId = @event.CustomerId,
            Description = $"Customer Payment Received - AR offset",
            CreatedAt = @event.PaymentDate,
            Lines = new List<JournalEntryLine>
            {
                new JournalEntryLine { AccountType = AccountType.Cash, Debit = @event.PaidAmount, Credit = 0 },
                new JournalEntryLine { AccountType = AccountType.AccountsReceivable, Debit = 0, Credit = @event.PaidAmount }
            }
        };

        await context.JournalEntries.AddAsync(journalEntry, cancellationToken);
    }
}
