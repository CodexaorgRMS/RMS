using System;

namespace Inventory.Application.Features.Products.Commands.Update
{
    public record UpdateProductCommand(Guid ProductId, string Name, string Description, Guid CategoryId);
}
