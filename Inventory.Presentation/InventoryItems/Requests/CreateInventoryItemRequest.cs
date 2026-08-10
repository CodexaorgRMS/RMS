using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Presentation.InventoryItems.Requests
{
    public sealed record CreateInventoryItemRequest(
    Guid ProductId,
    int Quantity,
    int MinStock
);
}
