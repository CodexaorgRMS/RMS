using FluentResults;
using Inventory.Application.Abstractions;
using Inventory.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Inventory.Commands;
using Wolverine.Attributes;

namespace Inventory.Application.Features.Stocks.CheckAvailability;

public static class CheckStockAvailabilityHandler
{
    public static async Task<Result> Handle(
        CheckStockAvailabilityCommand command,
        IInventoryDataContext context,
        CancellationToken cancellationToken)
    {
       
        foreach (var item in command.Items)
        {
            var availableQuantity = await context.ProductBatches
                .Where(b =>
                    b.ProductId == item.ProductId &&
                    b.Status == BatchStatus.Active &&
                    b.ExpiryDate > DateTime.UtcNow &&
                    b.CurrentQuantity > 0)
                .SumAsync(b => b.CurrentQuantity, cancellationToken);


            if (availableQuantity < item.Quantity)
            {
                return Result.Fail(
                    $"Insufficient stock for product {item.ProductId}. " +
                    $"Requested: {item.Quantity}, Available: {availableQuantity}");
            }
        }

        return Result.Ok();
    }
}