using FluentValidation;

namespace Sales.Presentation.Requests.Checkout;

public class CheckoutOrderRequestValidator : AbstractValidator<CheckoutOrderRequest>
{
	public CheckoutOrderRequestValidator()
	{
		RuleFor(x => x.PaidAmount)
			.GreaterThanOrEqualTo(0)
			.WithMessage("Paid amount must be greater than or equal to zero.");
	}
}
