using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Presentation.InventoryItems.Requests
{
    public sealed record DecreaseInventoryItemQuantityRequest(
     int Quantity
 );
}
