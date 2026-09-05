using Customers.Application.Abstractions;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using Wolverine.Attributes;

namespace Customers.Application.Features.Debts.Commands.AddManualCustomerDebt;

/// <summary>
/// Wolverine handler for manually adding a debt to a customer.
/// Delegates all state mutation to the <see cref="Customers.Domain.Entities.Customer.AddDebt"/> domain method.
/// </summary>
[Transactional]
public static class AddManualCustomerDebtHandler
{
	public static async Task<Result<Guid>> Handle(
		AddManualCustomerDebtCommand command,
		ICustomersDataContext context,
		CancellationToken cancellationToken)
	{
		var customer = await context.Customers
			.Include(c => c.Ledgers)
			.FirstOrDefaultAsync(c => c.CustomerId == command.CustomerId, cancellationToken);

		if (customer is null)
		{
			return Result.Fail<Guid>("Customer not found.");
		}

		// ── Delegate to domain method ──────────────────────────────────────
		var debtResult = customer.AddDebt(command.Amount, command.Reason, command.SourceOrderId);

		if (debtResult.IsFailed)
		{
			return Result.Fail<Guid>(debtResult.Errors);
		}

		return Result.Ok(debtResult.Value.LedgerId);
	}
}
