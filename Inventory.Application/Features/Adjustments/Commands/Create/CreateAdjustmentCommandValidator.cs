using FluentValidation;
using Inventory.Application.Abstractions;
using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Features.Adjustments.Commands.Create
{
    public class CreateAdjustmentCommandValidator : AbstractValidator<CreateAdjustmentCommand>
    {
        private readonly IInventoryDataContext _context;

        public CreateAdjustmentCommandValidator(IInventoryDataContext context)
        {
            _context = context;

            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("ProductId is required.")
                .MustAsync(ProductExists)
                .WithMessage("Product does not exist.");

            RuleFor(x => x.ProductBatchId)
                .NotEmpty()
                .WithMessage("ProductBatchId is required.")
                .MustAsync(BatchExists)
                .WithMessage("Product batch not found for the given ProductId.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than 0.");

            RuleFor(x => x.Type)
                .IsInEnum()
                .WithMessage("Invalid AdjustmentType.");

            RuleFor(x => x.Reason)
                .IsInEnum()
                .WithMessage("Invalid AdjustmentReason.");

            RuleFor(x => x)
                .MustAsync(HasSufficientQuantityForDecrease)
                .When(x => x.Type == AdjustmentType.Decrease)
                .WithMessage("Insufficient quantity in batch for decrease adjustment.");
        }

        private async Task<bool> ProductExists(Guid productId, CancellationToken cancellationToken)
        {
            return await _context.Products
                .AnyAsync(p => p.ProductId == productId, cancellationToken);
        }

        private async Task<bool> BatchExists(CreateAdjustmentCommand command, Guid batchId, CancellationToken cancellationToken)
        {
            return await _context.ProductBatches
                .AnyAsync(b => b.BatchId == batchId && b.ProductId == command.ProductId, cancellationToken);
        }

        private async Task<bool> HasSufficientQuantityForDecrease(CreateAdjustmentCommand command, CancellationToken cancellationToken)
        {
            return await _context.ProductBatches
                .AnyAsync(b => b.BatchId == command.ProductBatchId && b.CurrentQuantity >= command.Quantity, cancellationToken);
        }
    }
}
