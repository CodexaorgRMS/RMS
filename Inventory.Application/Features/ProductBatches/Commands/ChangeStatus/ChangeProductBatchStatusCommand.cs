using Inventory.Domain.Enums;

namespace Inventory.Application.Features.ProductBatches.Commands.ChangeStatus;

public sealed record ChangeProductBatchStatusCommand(
    Guid BatchId,
    BatchStatus Status,
    string? HoldReason);