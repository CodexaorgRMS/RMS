using FluentValidation;
using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Features.Categories.Commands.Delete;

public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
{
    private readonly IInventoryDataContext _context;

    public DeleteCategoryCommandValidator(IInventoryDataContext context)
    {
        _context = context;

        RuleFor(x => x.CategoryId)
            .NotEmpty()
            .WithMessage("CategoryId is required.")
            .MustAsync(CategoryExists)
            .WithMessage("Category not found.")
            .MustAsync(HasNoChildren)
            .WithMessage("Cannot delete category because it has child categories.")
            .MustAsync(HasNoProducts)
            .WithMessage("Cannot delete category because it contains products.");
    }

    private async Task<bool> CategoryExists(Guid categoryId, CancellationToken cancellationToken)
    {
        return await _context.Categories.AnyAsync(c => c.CategoryId == categoryId, cancellationToken);
    }

    private async Task<bool> HasNoChildren(Guid categoryId, CancellationToken cancellationToken)
    {
        return !await _context.Categories.AnyAsync(c => c.ParentId == categoryId, cancellationToken);
    }

    private async Task<bool> HasNoProducts(Guid categoryId, CancellationToken cancellationToken)
    {
        return !await _context.Products.AnyAsync(p => p.CategoryId == categoryId, cancellationToken);
    }
}
