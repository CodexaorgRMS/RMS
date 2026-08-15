using FluentValidation;
using Inventory.Domain.Enums;

namespace Inventory.Application.Features.ProductBatches.Commands.ChangeStatus;

public sealed class ChangeProductBatchStatusCommandValidator : AbstractValidator<ChangeProductBatchStatusCommand>
{
    public ChangeProductBatchStatusCommandValidator()
    {
        RuleFor(x => x.BatchId)
            .NotEmpty().WithMessage("BatchId is required.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Invalid batch status.");

        RuleFor(x => x.HoldReason)
            .NotEmpty()
            .When(x => x.Status is BatchStatus.OnHold or BatchStatus.Recalled)
            .WithMessage("A reason must be provided when placing a batch on hold or recalling it.")
            .MaximumLength(500).WithMessage("Reason must not exceed 500 characters.");
    }
}