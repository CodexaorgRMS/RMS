using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application.Features.InventoryItems.Commands.DecreaseQuantity
{
    public sealed record DecreaseInventoryItemQuantityCommand(
    Guid InventoryItemId,
    int Quantity
    );
}
