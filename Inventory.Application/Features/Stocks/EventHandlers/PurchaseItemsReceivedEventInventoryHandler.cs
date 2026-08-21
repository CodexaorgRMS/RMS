using FluentResults;
using FluentValidation.Results;
using Inventory.Application.Abstractions;
using Inventory.Application.Features.ProductBatches.Commands.Receive;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Purchases.Events;
using SharedKernel.Exeptions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Wolverine;

namespace Inventory.Application.Features.Stocks.EventHandlers;

public static class PurchaseItemsReceivedEventInventoryHandler
{
    public static async Task Handle(
        PurchaseItemsReceivedIntegrationEvent @event,
        IInventoryDataContext context,
        IMessageBus bus,
        CancellationToken cancellationToken)
    {
        foreach (var item in @event.Items)
        {
            var alreadyProcessed = await context.StockMovements
                .AsNoTracking()
                .AnyAsync(x => x.ReferenceId == @event.ReceiptId && x.ProductId == item.ProductId,
                    cancellationToken);

            if (alreadyProcessed)
                continue;

            var command = new ReceiveProductBatchCommand(
                item.ProductId, item.UnitCost, item.Quantity, item.ExpiryDate, @event.ReceiptId);

            var result = await bus.InvokeAsync<Result<System.Guid>>(command);

            if (result.IsFailed)
            {
                var failures = result.Errors
                    .Select(error => new ValidationFailure("ReceiveProductBatch", error.Message))
                    .ToList();

                throw new CommandValidationException(failures);
            }
        }
    }
}
