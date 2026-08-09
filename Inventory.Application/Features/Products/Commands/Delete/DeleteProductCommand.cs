using System;

namespace Inventory.Application.Features.Products.Commands.Delete
{
    public record DeleteProductCommand(Guid ProductId);
}
