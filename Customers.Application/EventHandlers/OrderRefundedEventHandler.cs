using Customers.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Sales.Events;
using Wolverine.Attributes;

namespace Customers.Application.EventHandlers;

/// <summary>
/// Handles the <see cref="OrderRefundedEvent"/> by reversing any outstanding debt
/// that was previously recorded for the order.
/// Delegates all state mutation to <see cref="Customers.Domain.Entities.Customer.AddPayment"/>.
/// </summary>
public static class OrderRefundedEventHandler
{
	[Transactional]
	public static async Task Handle(
		OrderRefundedEvent @event,
		ICustomersDataContext context,
		CancellationToken cancellationToken)
	{
		decimal debt = @event.TotalAmount - @event.PaidAmount;

		if (debt <= 0 || !@event.CustomerId.HasValue)
		{
			return;
		}

		var customerExists = await context.Customers
			.AnyAsync(c => c.CustomerId == @event.CustomerId.Value, cancellationToken);

		if (!customerExists)
		{
			return;
		}

		var ledger = new Customers.Domain.Entities.CustomerLedger
		{
			LedgerId = Guid.NewGuid(),
			CustomerId = @event.CustomerId.Value,
			Type = Customers.Domain.Enums.LedgerType.Payment,
			Amount = debt,
			Reason = "Order refund — debt reversal",
			ReferenceOrderId = @event.OrderId,
			CreatedAt = DateTime.UtcNow
		};

		// ── Update TotalDebt directly bypassing the change tracker ─────────
		await context.Customers
			.Where(c => c.CustomerId == @event.CustomerId.Value)
			.ExecuteUpdateAsync(s => s.SetProperty(c => c.TotalDebt, c => c.TotalDebt - debt), cancellationToken);

		// ── Insert Ledger directly ─────────────────────────────────────────
		context.CustomerLedgers.Add(ledger);
	}
}
