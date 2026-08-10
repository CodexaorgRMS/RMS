using System;
using System.Collections.Generic;
using System.Text;

namespace SharedContracts.Inventory.Events
{
    public sealed record InventoryItemOutOfStockEvent(
    Guid InventoryItemId,
    Guid ProductId,
    DateTime OccurredAt
    );
}
