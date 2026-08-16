using Customers.Application.Abstractions;
using Customers.Domain.Entities;
using Customers.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Sales.Events;

namespace Customers.Application.EventHandlers;

public static class OrderRefundedEventHandler
{
	public static async Task Handle(
		OrderRefundedEvent @event,
		ICustomersDataContext context,
		CancellationToken cancellationToken)
	{
		decimal debt = @event.TotalAmount - @event.PaidAmount;

		if (debt > 0 && @event.CustomerId.HasValue)
		{
			var customer = await context.Customers
				.FirstOrDefaultAsync(c => c.CustomerId == @event.CustomerId.Value, cancellationToken);

			if (customer is not null)
			{
				// Reverse the debt by recording a Payment-type ledger entry
				var ledger = new CustomerLedger
				{
					CustomerId = customer.CustomerId,
					Type = LedgerType.Payment,
					Amount = debt,
					ReferenceOrderId = @event.OrderId,
					CreatedAt = DateTime.UtcNow
				};

				customer.TotalDebt -= debt;

				context.CustomerLedgers.Add(ledger);
				await context.SaveChangesAsync(cancellationToken);
			}
		}
	}
}
