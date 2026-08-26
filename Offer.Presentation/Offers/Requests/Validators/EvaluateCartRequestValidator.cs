using FluentValidation;

namespace Offers.Presentation.Offers.Requests.Validators;

public sealed class CartItemRequestValidator : AbstractValidator<CartItemRequest>
{
    public CartItemRequestValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

        RuleFor(x => x.UnitPrice)
            .GreaterThanOrEqualTo(0).WithMessage("UnitPrice must be non-negative.");
    }
}

public sealed class EvaluateCartRequestValidator : AbstractValidator<EvaluateCartRequest>
{
    public EvaluateCartRequestValidator()
    {
        RuleFor(x => x.Items)
            .NotNull().WithMessage("Cart items cannot be null.")
            .NotEmpty().WithMessage("Cart must contain at least one item to evaluate offers.");

        RuleForEach(x => x.Items)
            .SetValidator(new CartItemRequestValidator());
    }
}
