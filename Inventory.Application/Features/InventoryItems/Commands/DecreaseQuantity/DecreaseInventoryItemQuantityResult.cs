using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application.Features.InventoryItems.Commands.DecreaseQuantity
{
    public sealed record DecreaseInventoryItemQuantityResult(
    Guid InventoryItemId,
    int Quantity,
    int MinStock);
}
