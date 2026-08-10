using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Presentation.StockMovements.Dtos;

public sealed class StockMovementDto
{
    public Guid MovementId { get; set; }

    public Guid ProductId { get; set; }

    public string ProductName { get; set; } = null!;

    public string Type { get; set; } = null!;

    public int Quantity { get; set; }

    public Guid ReferenceId { get; set; }

    public DateTime CreatedAt { get; set; }
}