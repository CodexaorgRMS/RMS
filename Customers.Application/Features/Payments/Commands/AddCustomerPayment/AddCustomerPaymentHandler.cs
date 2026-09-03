using Customers.Application.Abstractions;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Customers.Events;
using Wolverine;
using Wolverine.Attributes;

namespace Customers.Application.Features.Payments.Commands.AddCustomerPayment;

/// <summary>
/// Wolverine handler for recording a customer payment.
/// Delegates all state mutation to <see cref="Customers.Domain.Entities.Customer.AddPayment"/>.
/// </summary>
[Transactional]
public static class AddCustomerPaymentHandler
{
	public static async Task<Result<Guid>> Handle(
		AddCustomerPaymentCommand command,
		ICustomersDataContext context,
		IMessageBus messageBus,
		CancellationToken cancellationToken)
	{
		var customerExists = await context.Customers
			.AnyAsync(c => c.CustomerId == command.CustomerId, cancellationToken);

		if (!customerExists)
		{
			return Result.Fail<Guid>("Customer not found.");
		}

		if (command.PaidAmount <= 0)
		{
			return Result.Fail<Guid>("Payment amount must be greater than zero.");
		}

		var ledger = new global::Customers.Domain.Entities.CustomerLedger
		{
			LedgerId = Guid.NewGuid(),
			CustomerId = command.CustomerId,
			Type = global::Customers.Domain.Enums.LedgerType.Payment,
			Amount = command.PaidAmount,
			Reason = "Manual payment",
			ReferenceOrderId = null,
			CreatedAt = DateTime.UtcNow
		};

		// ── Update TotalDebt directly bypassing the change tracker ─────────
		await context.Customers
			.Where(c => c.CustomerId == command.CustomerId)
			.ExecuteUpdateAsync(s => s.SetProperty(c => c.TotalDebt, c => c.TotalDebt - command.PaidAmount), cancellationToken);

		// ── Insert Ledger directly ─────────────────────────────────────────
		context.CustomerLedgers.Add(ledger);

		await messageBus.PublishAsync(new CustomerPaymentReceivedEvent(
			command.CustomerId,
			command.PaidAmount,
			ledger.CreatedAt,
			command.orderNumber));

		if (!string.IsNullOrWhiteSpace(command.orderNumber))
		{
			await messageBus.PublishAsync(new CustomerPaymentAppliedToOrderEvent(
				command.orderNumber,
				command.PaidAmount));
		}

		return Result.Ok(ledger.LedgerId);
	}
}