using Inventory.Application.Features.Categories.Commands.Create;
using Inventory.Application.Features.Categories.Commands.Update;
using Inventory.Application.Features.Categories.Commands.UpdateExpiryRule;
using Inventory.Application.Features.Categories.Commands.UpdatePickingStrategy;
using Inventory.Presentation.Categories.Requests;
using Riok.Mapperly.Abstractions;

namespace Inventory.Presentation.Categories.Mappers
{
    [Mapper]
    public partial class CategoryMapper
    {
        public partial CreateCategoryCommand MapToCommand(CreateCategoryRequest request);
        public partial UpdateCategoryCommand MapToCommand(UpdateCategoryRequest request);
        public partial UpdateCategoryPickingStrategyCommand MapToCommand(UpdateCategoryPickingStrategyRequest request);
        public UpdateCategoryExpiryRuleCommand MapToCommand(Guid categoryId, UpdateCategoryExpiryRuleRequest request)
        {
            return new UpdateCategoryExpiryRuleCommand(
                categoryId,
                request.ExpiryWarningDays,
                request.AutoMarkdownPercentage);
        }
    }
}
