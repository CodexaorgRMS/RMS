using FluentValidation;

namespace Purchases.Application.Features.Purchases.Commands.Submit;

public sealed class SubmitPurchaseCommandValidator : AbstractValidator<SubmitPurchaseCommand>
{
    public SubmitPurchaseCommandValidator()
    {
        RuleFor(x => x.PurchaseId)
            .NotEmpty()
            .WithMessage("PurchaseId is required.");
    }
}
