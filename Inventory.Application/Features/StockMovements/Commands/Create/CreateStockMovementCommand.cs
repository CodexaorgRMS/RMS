using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application.Features.StockMovements.Commands.Create;

public sealed record CreateStockMovementCommand(
    Guid ProductId,
    string Type, // IN | OUT | ADJUST
    int Quantity,
    Guid ReferenceId
);