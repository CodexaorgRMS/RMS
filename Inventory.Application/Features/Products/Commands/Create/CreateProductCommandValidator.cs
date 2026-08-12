using FluentValidation;
using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Features.Products.Commands.Create
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        private readonly IInventoryDataContext _context;

        public CreateProductCommandValidator(IInventoryDataContext context)
        {
            _context = context;

            RuleFor(x => x.Name)
                .MustAsync(BeUniqueName)
                .WithMessage("Product name must be unique.");

            RuleFor(x => x.CategoryId)
                .MustAsync(CategoryExists)
                .WithMessage("Category does not exist.");
        }

        private async Task<bool> BeUniqueName(string name, CancellationToken cancellationToken)
        {
            return !await _context.Products.AnyAsync(p => p.Name == name, cancellationToken);
        }

        private async Task<bool> CategoryExists(System.Guid categoryId, CancellationToken cancellationToken)
        {
            return await _context.Categories.AnyAsync(c => c.CategoryId == categoryId, cancellationToken);
        }
    }
}
