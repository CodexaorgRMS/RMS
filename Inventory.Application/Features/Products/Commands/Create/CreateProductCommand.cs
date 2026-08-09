using System;

namespace Inventory.Application.Features.Products.Commands.Create
{
    public record CreateProductCommand(string Name, string Description, Guid CategoryId);
}
