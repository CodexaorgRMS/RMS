using FluentValidation;

namespace Customers.Application.Features.Debts.Commands.AddManualCustomerDebt;

public class AddManualCustomerDebtCommandValidator : AbstractValidator<AddManualCustomerDebtCommand>
{
	public AddManualCustomerDebtCommandValidator()
	{
		RuleFor(x => x.CustomerId)
			.NotEmpty().WithMessage("CustomerId is required.");

		RuleFor(x => x.Amount)
			.GreaterThan(0).WithMessage("Debt amount must be greater than zero.");
	}
}
