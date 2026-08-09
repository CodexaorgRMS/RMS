using FluentValidation;
using Inventory.Application.Abstractions;
using Inventory.Application.Features.Categories.Commands.Update;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Categories.Commands.Update
{
    public class UpdateCategoryCommandValidator : AbstractValidator<UpdateCategoryCommand>
    {
        private readonly IInventoryDbContext _context;

        public UpdateCategoryCommandValidator(IInventoryDbContext context)
        {
            _context = context;

            RuleFor(x => x.CategoryId)
                .Must((command, categoryId) => !command.ParentId.HasValue || command.ParentId.Value != categoryId)
                .WithMessage("A category cannot be its own parent.");

            RuleFor(x => x)
                .MustAsync(BeUniqueNameUnderParent)
                .WithMessage("Category name must be unique under the same parent.");

            RuleFor(x => x.ParentId)
                .MustAsync(ParentExists)
                .When(x => x.ParentId.HasValue)
                .WithMessage("Parent category does not exist.");

            RuleFor(x => x)
                .MustAsync(NotBeACircularHierarchy)
                .When(x => x.ParentId.HasValue)
                .WithMessage("Cannot create a circular hierarchy. The parent category cannot be a descendant of this category.");
        }

        private async Task<bool> BeUniqueNameUnderParent(UpdateCategoryCommand command, CancellationToken cancellationToken)
        {
            return !await _context.Categories
                .AnyAsync(c => c.Name == command.Name && c.ParentId == command.ParentId && c.CategoryId != command.CategoryId, cancellationToken);
        }

        private async Task<bool> ParentExists(System.Guid? parentId, CancellationToken cancellationToken)
        {
            if (!parentId.HasValue) return true;
            return await _context.Categories.AnyAsync(c => c.CategoryId == parentId.Value, cancellationToken);
        }

        private async Task<bool> NotBeACircularHierarchy(UpdateCategoryCommand command, CancellationToken cancellationToken)
        {
            if (!command.ParentId.HasValue) return true;
            if (command.CategoryId == command.ParentId.Value) return false;

            var currentParentId = command.ParentId.Value;

            while (true)
            {
                var parent = await _context.Categories
                    .AsNoTracking()
                    .FirstOrDefaultAsync(c => c.CategoryId == currentParentId, cancellationToken);

                if (parent == null || !parent.ParentId.HasValue)
                {
                    break;
                }

                if (parent.ParentId.Value == command.CategoryId)
                {
                    return false; // Circular reference detected
                }

                currentParentId = parent.ParentId.Value;
            }

            return true;
        }
    }
}
