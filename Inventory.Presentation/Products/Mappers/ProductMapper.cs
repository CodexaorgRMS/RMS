using Inventory.Application.Features.Categories.Commands.UpdateExpiryRule;
using Inventory.Application.Features.Products.Commands.Create;
using Inventory.Application.Features.Products.Commands.Update;
using Inventory.Application.Features.Products.Commands.UpdateExpiryRule;
using Inventory.Application.Features.Products.Commands.UpdatePickingStrategy;
using Inventory.Presentation.Categories.Requests;
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
        public UpdateProductExpiryRuleCommand MapToCommand(Guid productId, UpdateProductExpiryRuleRequest request)
        {
            return new UpdateProductExpiryRuleCommand(
                productId,
                request.CustomExpiryWarningDays,
                request.CustomAutoMarkdownPercentage);
        }
    }
}
