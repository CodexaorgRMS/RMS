using Inventory.Application.Features.StockMovements.Commands.Create;
using Inventory.Presentation.StockMovements.Requests;
using Riok.Mapperly.Abstractions;

namespace Inventory.Presentation.StockMovements.Mappers;

[Mapper]
public partial class StockMovementMapper
{
    public partial CreateStockMovementCommand MapToCommand(
        CreateStockMovementRequest request);
}
