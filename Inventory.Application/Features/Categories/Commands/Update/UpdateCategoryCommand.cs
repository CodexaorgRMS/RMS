using System;

namespace Inventory.Application.Features.Categories.Commands.Update
{
    public record UpdateCategoryCommand(Guid CategoryId, string Name, string Description, Guid? ParentId);
}
