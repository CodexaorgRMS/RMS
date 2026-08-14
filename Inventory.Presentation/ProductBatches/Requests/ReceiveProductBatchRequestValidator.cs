using FluentValidation;
using System;

namespace Inventory.Presentation.ProductBatches.Requests
{
    public class ReceiveProductBatchRequestValidator : AbstractValidator<ReceiveProductBatchRequest>
    {
        public ReceiveProductBatchRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("Product Id is required.");

            RuleFor(x => x.CostPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Cost price must be greater than or equal to 0.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.");

            RuleFor(x => x.ExpiryDate)
                .NotEmpty()
                .WithMessage("Expiry date is required.")
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Expiry date must be in the future.");
        }
    }
}
