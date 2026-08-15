using Customers.Application.Abstractions;
using Customers.Domain.Entities;
using Customers.Domain.Enums;
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

		if (debt > 0 )
		{
			var customer = await context.Customers
				.FirstOrDefaultAsync(c => c.CustomerId == @event.CustomerId, cancellationToken);

			if (customer != null)
			{
				var ledger = new CustomerLedger
				{
					LedgerId = Guid.NewGuid(),
					CustomerId = customer.CustomerId,
					Type = LedgerType.Debt,
					Amount = debt,
					ReferenceOrderId = @event.OrderId,
					CreatedAt = DateTime.UtcNow
				};

				customer.TotalDebt += debt;

				context.CustomerLedgers.Add(ledger);
				await context.SaveChangesAsync(cancellationToken);
			}
		}
	}
}
