using Inventory.Application.Features.Categories.Commands.Create;
using Inventory.Application.Features.Categories.Commands.Update;
using Inventory.Presentation.Categories.Requests;
using Riok.Mapperly.Abstractions;

namespace Inventory.Presentation.Categories.Mappers
{
    [Mapper]
    public partial class CategoryMapper
    {
        public partial CreateCategoryCommand MapToCommand(CreateCategoryRequest request);
        public partial UpdateCategoryCommand MapToCommand(UpdateCategoryRequest request);
    }
}
