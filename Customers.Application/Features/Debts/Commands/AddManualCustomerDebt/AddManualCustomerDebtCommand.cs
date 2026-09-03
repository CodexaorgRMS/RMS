namespace Customers.Application.Features.Debts.Commands.AddManualCustomerDebt;

public record AddManualCustomerDebtCommand(
	Guid CustomerId,
	decimal Amount,
	string? Reason,
	Guid? SourceOrderId = null);
