using Customers.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Sales.Events;

namespace Customers.Application.EventHandlers;

public static class OrderCompletedEventHandler
{
	public static async Task Handle(
		OrderCompletedEvent @event,
		ICustomersDataContext context,
		CancellationToken cancellationToken)
	{
		decimal debt = @event.TotalAmount - @event.PaidAmount;

		if (debt <= 0 || !@event.CustomerId.HasValue)
		{
			return;
		}

		// ── Idempotency guard: don't process the same order twice ──────────
		var alreadyProcessed = await context.CustomerLedgers
			.AnyAsync(l => l.ReferenceOrderId == @event.OrderId, cancellationToken);

		if (alreadyProcessed)
		{
			return;
		}

		// ── Update TotalDebt directly bypassing the change tracker ─────────
		await context.Customers
			.Where(c => c.CustomerId == @event.CustomerId.Value)
			.ExecuteUpdateAsync(s => s.SetProperty(c => c.TotalDebt, c => c.TotalDebt + debt), cancellationToken);

		// ── Insert Ledger directly ─────────────────────────────────────────
		var ledger = new Customers.Domain.Entities.CustomerLedger
		{
			LedgerId = Guid.NewGuid(),
			CustomerId = @event.CustomerId.Value,
			Type = Customers.Domain.Enums.LedgerType.Debt,
			Amount = debt,
			Reason = "POS Checkout — outstanding balance",
			ReferenceOrderId = @event.OrderId,
			CreatedAt = DateTime.UtcNow
		};

		context.CustomerLedgers.Add(ledger);

		try
		{
			await context.SaveChangesAsync(cancellationToken);
		}
		catch (DbUpdateException ex)
		{
			Console.WriteLine($"CUSTOMERS DB UPDATE EXCEPTION! Inner: {ex.InnerException?.Message}");
			throw;
		}
	}
}
