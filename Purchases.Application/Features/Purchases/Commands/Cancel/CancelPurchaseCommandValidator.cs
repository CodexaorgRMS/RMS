using FluentValidation;

namespace Purchases.Application.Features.Purchases.Commands.Cancel;

public sealed class CancelPurchaseCommandValidator : AbstractValidator<CancelPurchaseCommand>
{
    public CancelPurchaseCommandValidator()
    {
        RuleFor(x => x.PurchaseId)
            .NotEmpty()
            .WithMessage("PurchaseId is required.");
    }
}
