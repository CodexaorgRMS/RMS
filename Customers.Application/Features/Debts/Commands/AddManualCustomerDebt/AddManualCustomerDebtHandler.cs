using Customers.Application.Abstractions;
using Customers.Domain.Entities;
using Customers.Domain.Enums;
using FluentResults;
using Microsoft.EntityFrameworkCore;

namespace Customers.Application.Features.Debts.Commands.AddManualCustomerDebt;

public static class AddManualCustomerDebtHandler
{
	public static async Task<Result<Guid>> Handle(
		AddManualCustomerDebtCommand command,
		ICustomersDataContext context,
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
			Type = LedgerType.Debt,
			Amount = command.Amount,
			ReferenceOrderId = null,
			CreatedAt = DateTime.UtcNow
		};

		customer.TotalDebt += command.Amount;

		context.CustomerLedgers.Add(ledger);
		await context.SaveChangesAsync(cancellationToken);

		return Result.Ok(ledger.LedgerId);
	}
}
