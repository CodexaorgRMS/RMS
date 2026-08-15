
using BatchStatus = Inventory.Domain.Enums.BatchStatus;

namespace Inventory.Presentation.ProductBatches.Requests;

public sealed record ChangeProductBatchStatusRequest(
    BatchStatus Status,
    string? HoldReason);