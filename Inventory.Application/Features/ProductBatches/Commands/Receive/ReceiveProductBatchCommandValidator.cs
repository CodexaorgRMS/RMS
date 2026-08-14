using FluentValidation;
using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Features.ProductBatches.Commands.Receive
{
    public class ReceiveProductBatchCommandValidator : AbstractValidator<ReceiveProductBatchCommand>
    {
        private readonly IInventoryDataContext _context;

        public ReceiveProductBatchCommandValidator(IInventoryDataContext context)
        {
            _context = context;

            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("Product ID is required.")
                .MustAsync(ProductExists)
                .WithMessage("Product does not exist.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero.");

            RuleFor(x => x.CostPrice)
                .GreaterThanOrEqualTo(0)
                .WithMessage("Cost price must be greater than or equal to zero.");

            RuleFor(x => x.ExpiryDate)
                .GreaterThan(DateTime.UtcNow)
                .WithMessage("Expiry date must be in the future.");
        }

        private async Task<bool> ProductExists(Guid productId, CancellationToken cancellationToken)
        {
            return await _context.Products.AnyAsync(p => p.ProductId == productId, cancellationToken);
        }
    }
}
