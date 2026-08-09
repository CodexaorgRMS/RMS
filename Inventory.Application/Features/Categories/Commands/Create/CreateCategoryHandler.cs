using Inventory.Application.Abstractions;
using Inventory.Application.Features.Categories.Commands.Create;
using Inventory.Domain.Entities;
using System;
using System.Threading.Tasks;
using Wolverine.Attributes;

namespace Inventory.Application.Features.Categories.Commands.Create
{
    [Transactional]
    public static class CreateCategoryHandler
    {

        public static async Task<Guid> Handle(CreateCategoryCommand command, IInventoryDbContext _context)
        {
            var category = new Category
            {
                CategoryId = Guid.NewGuid(),
                Name = command.Name,
                Description = command.Description,
                ParentId = command.ParentId
            };

           await _context.Categories.AddAsync(category);

            return category.CategoryId;
        }
    }
}
