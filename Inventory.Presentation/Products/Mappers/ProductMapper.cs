using Inventory.Application.Features.Products.Commands.Create;
using Inventory.Application.Features.Products.Commands.Update;
using Inventory.Application.Features.Products.Commands.UpdatePickingStrategy;
using Inventory.Presentation.Products.Requests;
using Riok.Mapperly.Abstractions;

namespace Inventory.Presentation.Products.Mappers
{
    [Mapper]
    public partial class ProductMapper
    {
        public partial CreateProductCommand MapToCommand(CreateProductRequest request);
        public partial UpdateProductCommand MapToCommand(UpdateProductRequest request);
        public partial UpdateProductPickingStrategyCommand MapToCommand(UpdateProductPickingStrategyRequest request);
	}
}
