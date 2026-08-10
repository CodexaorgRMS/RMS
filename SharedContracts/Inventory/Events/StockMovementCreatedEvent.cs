using System;
using System.Collections.Generic;
using System.Text;

namespace SharedContracts.Inventory.Events;

public sealed record StockMovementCreatedEvent(
    Guid MovementId,
    Guid ProductId,
    string Type,
    int Quantity,
    Guid ReferenceId,
    DateTime CreatedAt
);