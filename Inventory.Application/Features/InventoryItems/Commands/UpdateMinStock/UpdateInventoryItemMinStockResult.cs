using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application.Features.InventoryItems.Commands.UpdateMinStock
{
    public sealed record UpdateInventoryItemMinStockResult(
    Guid InventoryItemId,
    int Quantity,
    int MinStock);
}
