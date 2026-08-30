using System;
using System.Collections.Generic;

namespace SharedContracts.Inventory.Commands
{
    public record CheckStockAvailabilityCommand(
        IReadOnlyCollection<StockCheckItemDto> Items
    );

    public record StockCheckItemDto(
        Guid ProductId,
        int Quantity
    );
}