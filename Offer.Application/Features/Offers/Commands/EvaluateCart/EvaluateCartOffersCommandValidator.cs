using FluentValidation;

namespace Offers.Application.Features.Offers.Commands.EvaluateCart;

public sealed class EvaluateCartOffersCommandValidator : AbstractValidator<EvaluateCartOffersCommand>
{
    public EvaluateCartOffersCommandValidator()
    {
        RuleFor(x => x.Items)
            .NotNull().WithMessage("Cart items list cannot be null.")
            .NotEmpty().WithMessage("Cart must contain at least one item to evaluate offers.");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.ProductId)
                .NotEmpty().WithMessage("ProductId is required.");

            item.RuleFor(i => i.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

            item.RuleFor(i => i.UnitPrice)
                .GreaterThanOrEqualTo(0).WithMessage("UnitPrice must be non-negative.");
        });
    }
}
