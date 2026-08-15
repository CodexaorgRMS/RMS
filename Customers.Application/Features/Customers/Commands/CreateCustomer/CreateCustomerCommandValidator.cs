using FluentValidation;

namespace Customers.Application.Features.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandValidator : AbstractValidator<CreateCustomerCommand>
{
	public CreateCustomerCommandValidator()
	{
		RuleFor(x => x.Name)
			.NotEmpty().WithMessage("Customer name is required.")
			.MaximumLength(150).WithMessage("Customer name cannot exceed 150 characters.");

		RuleFor(x => x.Phone)
			.NotEmpty().WithMessage("Customer phone number is required.")
			.MaximumLength(20).WithMessage("Phone number cannot exceed 20 characters.");
	}
}
