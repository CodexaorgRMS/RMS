using FluentResults;
using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions;
using Sales.Domain.Enums;
using SharedContracts.Sales.Events;
using SharedContracts.Inventory.Commands;
using SharedContracts.Offers.Interfaces;
using Wolverine;
using Wolverine.Attributes;

namespace Sales.Application.Features.Checkout;

[Transactional]
public static class CheckoutOrderCommandHandler
{
    public static async Task<Result> Handle(
        CheckoutOrderCommand command,
        ISalesDataContext context,
        IMessageBus bus,
        /*IOfferService offerService,*/
        CancellationToken cancellationToken)
    {
        // Fetch the Order along with its OrderItems.
        var order = await context.Orders
            .Include(o => o.Items)
            .FirstAsync(o => o.OrderId == command.orderId, cancellationToken);

        var stockCheckItems = order.Items
            .Select(item => new StockCheckItemDto(
                item.ProductId,
                item.Quantity))
            .ToList();

        var stockResult = await bus.InvokeAsync<Result>(new CheckStockAvailabilityCommand(stockCheckItems), cancellationToken);
        if (stockResult.IsFailed)
        {
            return stockResult;
        }

        var soldItems = order.Items.Select(item =>
            new SoldItemDto(item.ProductId, item.Quantity)).ToList();

        //var discountAmount = await offerService.CalculateDiscountAsync(soldItems, cancellationToken);

        order.SubTotal = order.Items.Sum(i => i.TotalPrice);
        //order.DiscountAmount = discountAmount;
        order.TotalAmount = order.SubTotal - order.DiscountAmount;

        order.PaidAmount = command.PaidAmount;
        order.CustomerId = command.CustomerId;
        order.Status = OrderStatus.Completed;

        await bus.PublishAsync(new OrderCompletedEvent(
            OrderId: order.OrderId,
            CustomerId: command.CustomerId,
            TotalAmount: order.TotalAmount,
            PaidAmount: command.PaidAmount,
            Items: soldItems,
            CompletedAt: DateTime.UtcNow));

        return Result.Ok();
    }
}
