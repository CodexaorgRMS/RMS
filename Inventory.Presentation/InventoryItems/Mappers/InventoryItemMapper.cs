using Inventory.Application.Features.InventoryItems.Commands.Create;
using Inventory.Application.Features.InventoryItems.Commands.Create;
using Inventory.Presentation.InventoryItems.Requests;
using Riok.Mapperly.Abstractions;

namespace Inventory.Presentation.InventoryItems.Mappers;

[Mapper]
public partial class InventoryItemMapper
{
    public partial CreateInventoryItemCommand MapToCommand(
        CreateInventoryItemRequest request);

}