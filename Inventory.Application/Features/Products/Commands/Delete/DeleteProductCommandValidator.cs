using FluentValidation;
using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Features.Products.Commands.Delete;

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    private readonly IInventoryDataContext _context;

    public DeleteProductCommandValidator(IInventoryDataContext context)
    {
        _context = context;

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("ProductId is required.")
            .MustAsync(ProductExists)
            .WithMessage("Product not found.");
    }

    private async Task<bool> ProductExists(Guid productId, CancellationToken cancellationToken)
    {
        return await _context.Products.AnyAsync(p => p.ProductId == productId, cancellationToken);
    }
}
