using System;

namespace Inventory.Application.Features.Categories.Commands.Create
{
    public record CreateCategoryCommand(string Name, string Description, Guid? ParentId);
}
