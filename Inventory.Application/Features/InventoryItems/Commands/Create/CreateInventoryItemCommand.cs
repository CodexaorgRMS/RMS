using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application.Features.InventoryItems.Commands.Create
{
    public sealed record CreateInventoryItemCommand(
    Guid ProductId,
    int Quantity,
    int MinStock
);
}
