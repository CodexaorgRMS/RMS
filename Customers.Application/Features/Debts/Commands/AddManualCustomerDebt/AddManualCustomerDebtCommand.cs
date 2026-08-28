namespace Customers.Application.Features.Debts.Commands.AddManualCustomerDebt;

public record AddManualCustomerDebtCommand(Guid CustomerId, decimal Amount,
	Guid? RefrenceOrderId,
	string? Reason);
