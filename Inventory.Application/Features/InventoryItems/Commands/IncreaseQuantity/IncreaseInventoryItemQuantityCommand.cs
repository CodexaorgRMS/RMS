using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application.Features.InventoryItems.Commands.IncreaseQuantity
{
    public sealed record IncreaseInventoryItemQuantityCommand(
        Guid InventoryItemId,
        int Quantity
    );
}
