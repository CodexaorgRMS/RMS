using FluentValidation;
using Inventory.Application.Abstractions;
using Inventory.Application.Features.Products.Commands.Update;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Products.Commands.Update
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        private readonly IInventoryDbContext _context;

        public UpdateProductCommandValidator(IInventoryDbContext context)
        {
            _context = context;

            RuleFor(x => x.Name)
                .MustAsync(async (command, name, cancellationToken) => await BeUniqueName(command, name, cancellationToken))
                .WithMessage("Product name must be unique.");

            RuleFor(x => x.CategoryId)
                .MustAsync(CategoryExists)
                .WithMessage("Category does not exist.");
        }

        private async Task<bool> BeUniqueName(UpdateProductCommand command, string name, CancellationToken cancellationToken)
        {
            return !await _context.Products.AnyAsync(p => p.Name == name && p.ProductId != command.ProductId, cancellationToken);
        }

        private async Task<bool> CategoryExists(System.Guid categoryId, CancellationToken cancellationToken)
        {
            return await _context.Categories.AnyAsync(c => c.CategoryId == categoryId, cancellationToken);
        }
    }
}
