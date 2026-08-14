using FluentValidation;
using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Features.Products.Commands.Update
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        private readonly IInventoryDataContext _context;

        public UpdateProductCommandValidator(IInventoryDataContext context)
        {
            _context = context;

            RuleFor(x => x.ProductId)
                .NotEmpty()
                .WithMessage("ProductId is required.")
                .MustAsync(ProductExists)
                .WithMessage("Product not found.");

            RuleFor(x => x.Name)
                .NotEmpty()
                .WithMessage("Product name is required.")
                .MustAsync(BeUniqueName)
                .WithMessage("Product name must be unique.");

            RuleFor(x => x.CategoryId)
                .NotEmpty()
                .WithMessage("CategoryId is required.")
                .MustAsync(CategoryExists)
                .WithMessage("Category does not exist.");
        }

        private async Task<bool> ProductExists(Guid productId, CancellationToken cancellationToken)
        {
            return await _context.Products.AnyAsync(p => p.ProductId == productId, cancellationToken);
        }

        private async Task<bool> BeUniqueName(UpdateProductCommand command, string name, CancellationToken cancellationToken)
        {
            return !await _context.Products.AnyAsync(p => p.Name == name && p.ProductId != command.ProductId, cancellationToken);
        }

        private async Task<bool> CategoryExists(Guid categoryId, CancellationToken cancellationToken)
        {
            return await _context.Categories.AnyAsync(c => c.CategoryId == categoryId, cancellationToken);
        }
    }
}
