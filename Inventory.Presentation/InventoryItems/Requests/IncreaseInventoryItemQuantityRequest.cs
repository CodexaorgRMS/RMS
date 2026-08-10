using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Presentation.InventoryItems.Requests
{
    public sealed record IncreaseInventoryItemQuantityRequest(
     int Quantity
    );
}
