using FluentValidation;
using BatchStatus = Inventory.Domain.Enums.BatchStatus;

namespace Inventory.Presentation.ProductBatches.Requests;

public sealed class ChangeProductBatchStatusRequestValidator : AbstractValidator<ChangeProductBatchStatusRequest>
{
    public ChangeProductBatchStatusRequestValidator()
    {
        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid batch status value.");

        RuleFor(x => x.HoldReason)
            .NotEmpty()
            .When(x => x.Status is BatchStatus.OnHold or BatchStatus.Recalled)
            .WithMessage("HoldReason is required when status is OnHold or Recalled.")
            .MaximumLength(500).WithMessage("HoldReason must not exceed 500 characters.");
    }
}