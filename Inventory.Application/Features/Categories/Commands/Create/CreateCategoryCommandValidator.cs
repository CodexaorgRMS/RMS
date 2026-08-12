using FluentValidation;
using Inventory.Application.Abstractions;
using Inventory.Application.Features.Categories.Commands.Create;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Inventory.Application.Features.Categories.Commands.Create
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        private readonly IInventoryDataContext _context;

        public CreateCategoryCommandValidator(IInventoryDataContext context)
        {
            _context = context;

            RuleFor(x => x)
                .MustAsync(BeUniqueNameUnderParent)
                .WithMessage("Category name must be unique under the same parent.");

            RuleFor(x => x.ParentId)
                .MustAsync(ParentExists)
                .When(x => x.ParentId.HasValue)
                .WithMessage("Parent category does not exist.");
        }

        private async Task<bool> BeUniqueNameUnderParent(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            return !await _context.Categories
                .AnyAsync(c => c.Name == command.Name && c.ParentId == command.ParentId, cancellationToken);
        }

        private async Task<bool> ParentExists(System.Guid? parentId, CancellationToken cancellationToken)
        {
            if (!parentId.HasValue) return true;
            return await _context.Categories.AnyAsync(c => c.CategoryId == parentId.Value, cancellationToken);
        }
    }
}
