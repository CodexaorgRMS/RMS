using FluentValidation;

namespace Sales.Presentation.Requests.AddItem
{
	public class AddOrderItemRequestValidator : AbstractValidator<AddOrderItemRequest>
	{
		public AddOrderItemRequestValidator()
		{
			RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required.");
			RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than 0.");
		}
	}
}
