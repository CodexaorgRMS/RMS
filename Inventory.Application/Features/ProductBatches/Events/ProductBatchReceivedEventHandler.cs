using Inventory.Application.Features.ProductBatches.Events;

namespace Inventory.Application.Features.ProductBatches.Events;

public static class ProductBatchReceivedEventHandler
{
    public static Task Handle(
        ProductBatchReceivedEvent @event,
        CancellationToken cancellationToken)
    {
        Console.WriteLine(
            $"Product batch received. BatchId: {@event.BatchId}, ProductId: {@event.ProductId}");

        return Task.CompletedTask;
    }
}