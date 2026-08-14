using Inventory.Application.Features.ProductBatches.Commands.Receive;
using Inventory.Presentation.ProductBatches.Requests;
using Riok.Mapperly.Abstractions;

namespace Inventory.Presentation.ProductBatches.Mappers
{
    [Mapper]
    public partial class ProductBatchMapper
    {
        public partial ReceiveProductBatchCommand MapToCommand(ReceiveProductBatchRequest request);
    }
}
