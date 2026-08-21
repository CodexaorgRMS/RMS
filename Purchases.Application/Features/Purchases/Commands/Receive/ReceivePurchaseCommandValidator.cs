using FluentValidation;
using System;

namespace Purchases.Application.Features.Purchases.Commands.Receive;

public sealed class ReceivePurchaseCommandValidator : AbstractValidator<ReceivePurchaseCommand>
{
    public ReceivePurchaseCommandValidator()
    {
        RuleFor(x => x.PurchaseId)
            .NotEmpty()
            .WithMessage("PurchaseId is required.");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Receive items cannot be empty.");

        RuleFor(x => x.Items)
    .Must(items => items
        .Select(i => i.PurchaseItemId)
        .Distinct()
        .Count() == items.Count)
    .WithMessage("Duplicate PurchaseItemId is not allowed within the same receive request.");

        RuleForEach(x => x.Items)
            .ChildRules(item =>
            {
                item.RuleFor(i => i.PurchaseItemId)
                    .NotEmpty()
                    .WithMessage("PurchaseItemId is required.");

                item.RuleFor(i => i.ReceivedQuantity)
                    .GreaterThan(0)
                    .WithMessage("Received quantity must be greater than zero.");

                item.RuleFor(i => i.ExpiryDate)
                    .GreaterThan(DateTime.UtcNow)
                    .WithMessage("Expiry date must be in the future.");
            });
    }
}
