using FluentValidation;

namespace Customers.Application.Features.Payments.Commands.AddCustomerPayment;

public class AddCustomerPaymentCommandValidator : AbstractValidator<AddCustomerPaymentCommand>
{
	public AddCustomerPaymentCommandValidator()
	{
		RuleFor(x => x.CustomerId)
			.NotEmpty().WithMessage("CustomerId is required.");

		RuleFor(x => x.PaidAmount)
			.GreaterThan(0).WithMessage("PaidAmount must be greater than zero.");
	}
}
