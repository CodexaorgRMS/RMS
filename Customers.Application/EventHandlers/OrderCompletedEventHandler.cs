using Customers.Application.Abstractions;
using Customers.Domain.Entities;
using Customers.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Sales.Events;
using Wolverine.Attributes;

namespace Customers.Application.EventHandlers;

public static class OrderCompletedEventHandler
{
	[Transactional]
	public static async Task Handle(
		OrderCompletedEvent @event,
		ICustomersDataContext context,
		CancellationToken cancellationToken)
	{
		decimal debt = @event.TotalAmount - @event.PaidAmount;

		if (debt > 0 && @event.CustomerId.HasValue)
		{
			var alreadyProcessed = await context.CustomerLedgers
				.AnyAsync(l => l.ReferenceOrderId == @event.OrderId, cancellationToken);

			if (alreadyProcessed)
			{
				return;
			}

			var customer = await context.Customers
				.FirstOrDefaultAsync(c => c.CustomerId == @event.CustomerId.Value, cancellationToken);

			if (customer is not null)
			{
				var ledger = new CustomerLedger
				{
					CustomerId = customer.CustomerId,
					Type = LedgerType.Debt,
					Amount = debt,
					ReferenceOrderId = @event.OrderId,
					CreatedAt = DateTime.UtcNow
				};

				customer.TotalDebt += debt;

				context.CustomerLedgers.Add(ledger);
			}
		}
	}
}
