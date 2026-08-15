using Customers.Application.Abstractions;
using Customers.Domain.Entities;
using Customers.Domain.Enums;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Customers.Events;
using Wolverine;

namespace Customers.Application.Features.Payments.Commands.AddCustomerPayment;

public static class AddCustomerPaymentHandler
{
	public static async Task<Result<Guid>> Handle(
		AddCustomerPaymentCommand command,
		ICustomersDataContext context,
		IMessageBus messageBus,
		CancellationToken cancellationToken)
	{
		var customer = await context.Customers
			.FirstOrDefaultAsync(c => c.CustomerId == command.CustomerId, cancellationToken);

		if (customer is null)
		{
			return Result.Fail<Guid>("Customer not found.");
		}

		var ledger = new CustomerLedger
		{
			CustomerId = customer.CustomerId,
			Type = LedgerType.Payment,
			Amount = command.PaidAmount,
			ReferenceOrderId = null,
			CreatedAt = DateTime.UtcNow
		};

		customer.TotalDebt -= command.PaidAmount;

		context.CustomerLedgers.Add(ledger);
		await context.SaveChangesAsync(cancellationToken);

		// Publish integration event for cross-module async processing (e.g. Finance Module)
		await messageBus.PublishAsync(new CustomerPaymentReceivedEvent(
			customer.CustomerId,
			command.PaidAmount,
			DateTime.UtcNow));

		return Result.Ok(ledger.LedgerId);
	}
}
