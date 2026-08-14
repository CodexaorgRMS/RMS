using Inventory.Application.Features.Adjustments.Commands.Create;
using Inventory.Presentation.Adjustments.Requests;
using Riok.Mapperly.Abstractions;

namespace Inventory.Presentation.Adjustments.Mappers
{
    [Mapper]
    public partial class AdjustmentMapper
    {
        public partial CreateAdjustmentCommand MapToCommand(CreateAdjustmentRequest request);
    }
}
