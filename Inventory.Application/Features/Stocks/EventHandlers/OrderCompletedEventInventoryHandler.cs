using FluentResults;
using FluentValidation.Results;
using Inventory.Application.Abstractions;
using SharedContracts.Inventory.Commands;
using SharedContracts.Sales.Events;
using SharedKernel.Exeptions;
using Wolverine;
using Wolverine.Attributes;

namespace Inventory.Application.Features.Stocks.EventHandlers;

public static class OrderCompletedEventInventoryHandler
{
	// [Transactional]
	// public static async Task Handle(
	// 	OrderCompletedEvent @event,
	// 	IMessageBus bus,
	// 	IInventoryDataContext context)
	// {
	// 	/* 
    //      CRITICAL FIX: This legacy handler was disabled because stock deduction 
    //      is now securely handled by DeductStockForCheckoutHandler (CheckoutSaga Step 2).
    //      Leaving this active would cause duplicate stock deduction on every sale.
    //      */
	// }
}
