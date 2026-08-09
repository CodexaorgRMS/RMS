using System;

namespace Inventory.Application.Features.Categories.Commands.Delete
{
    public record DeleteCategoryCommand(Guid CategoryId);
}
