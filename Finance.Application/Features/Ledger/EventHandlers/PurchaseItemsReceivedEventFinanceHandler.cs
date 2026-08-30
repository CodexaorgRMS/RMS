using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Finance.Application.Abstractions;
using Finance.Domain.Entities;
using Finance.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Purchases.Events;
using Wolverine.Attributes;

namespace Finance.Application.Features.Ledger.EventHandlers;

public static class PurchaseItemsReceivedEventFinanceHandler
{
    [Transactional]
    public static async Task Handle(
        PurchaseItemsReceivedIntegrationEvent @event,
        IFinanceDataContext context,
        CancellationToken cancellationToken)
    {
        // Issue 4 & 8: Idempotency check for missing AP cash integration
        var alreadyProcessed = await context.JournalEntries
            .AnyAsync(j => j.ReferenceId == @event.ReceiptId, cancellationToken);

        if (alreadyProcessed)
        {
            return;
        }

        // Calculate total cost of items received
        decimal totalCost = @event.Items.Sum(i => i.UnitCost * i.Quantity);

        var cashMovement = new CashMovement
        {
            MovementId = Guid.NewGuid(),
            Amount = totalCost,
            Type = CashMovementType.Outbound,
            Source = PaymentSource.Purchasing,
            ReferenceId = @event.ReceiptId,
            Description = $"Cash disbursement for Purchase Receipt {@event.ReceiptId}",
            CreatedAt = @event.ReceivedAt
        };

        await context.CashMovements.AddAsync(cashMovement, cancellationToken);

        var journalEntry = new JournalEntry
        {
            JournalEntryId = Guid.NewGuid(),
            ReferenceId = @event.ReceiptId,
            Description = $"Supplier Payment & Inventory Asset for Purchase {@event.PurchaseOrderId}",
            CreatedAt = @event.ReceivedAt,
            Lines = new List<JournalEntryLine>
            {
                new JournalEntryLine { AccountType = AccountType.InventoryAsset, Debit = totalCost, Credit = 0 },
                new JournalEntryLine { AccountType = AccountType.Cash, Debit = 0, Credit = totalCost }
            }
        };

        await context.JournalEntries.AddAsync(journalEntry, cancellationToken);
    }
}
