using FluentValidation;

namespace Inventory.Presentation.Adjustments.Requests
{
    public class CreateAdjustmentRequestValidator : AbstractValidator<CreateAdjustmentRequest>
    {
        public CreateAdjustmentRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("ProductId is required.");

            RuleFor(x => x.ProductBatchId)
                .NotEmpty()
                .WithMessage("ProductBatchId is required.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.");

            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage("Invalid AdjustmentType.");

            RuleFor(x => x.Reason)
                .IsInEnum()
                .WithMessage("Invalid AdjustmentReason.");
        }
    }
}
