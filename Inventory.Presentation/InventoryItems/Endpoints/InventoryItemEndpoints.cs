using FluentResults;
using Inventory.Application.Features.InventoryItems.Commands.DecreaseQuantity;
using Inventory.Presentation.Extentions;
using Inventory.Presentation.InventoryItems.Mappers;
using Inventory.Presentation.InventoryItems.Requests;
using Microsoft.AspNetCore.Http;
using Wolverine;
using Wolverine.Http;

namespace Inventory.Presentation.InventoryItems.Endpoints;

public static class InventoryItemEndpoints
{
    [WolverinePost("/api/inventory-items")]
    public static async Task<IResult> Create(
        CreateInventoryItemRequest request,
        IMessageBus bus,
        InventoryItemMapper mapper,
        CancellationToken cancellationToken)
    {
        var command = mapper.MapToCommand(request);

        var result = await bus.InvokeAsync<FluentResults.Result<Guid>>(
            command,
            cancellationToken);

        if (result.IsFailed)
        {
            return Results.Problem(
                detail: string.Join(
                    "; ",
                    result.Errors.Select(x => x.Message)),
                statusCode: StatusCodes.Status400BadRequest);
        }

        return Results.Created(
            $"/api/inventory-items/{result.Value}",
            result.Value);
    }

    [WolverinePut("/api/inventory-items/{inventoryItemId}/min-stock")]
    public static async Task<IResult> UpdateMinStock(
    Guid inventoryItemId,
    UpdateInventoryItemMinStockRequest request,
    IMessageBus bus,
    InventoryItemMapper mapper,
    CancellationToken cancellationToken)
    {
        var command = new Inventory.Application.Features.InventoryItems.Commands.UpdateMinStock.UpdateInventoryItemMinStockCommand(
            inventoryItemId,
            request.MinStock);

        var result = await bus.InvokeAsync<Result>(
            command,
            cancellationToken);

        return result.ToHttpResult();
    }

    [WolverinePost("/api/inventory-items/{inventoryItemId}/increase")]
    public static async Task<IResult> IncreaseQuantity(
    Guid inventoryItemId,
    IncreaseInventoryItemQuantityRequest request,
    IMessageBus bus,
    InventoryItemMapper mapper,
    CancellationToken cancellationToken)
    {
        var command = new Inventory.Application.Features.InventoryItems.Commands.IncreaseQuantity.IncreaseInventoryItemQuantityCommand(
            inventoryItemId,
            request.Quantity);

        var result = await bus.InvokeAsync<Result>(
            command,
            cancellationToken);

        return result.ToHttpResult();
    }

    [WolverinePost("/api/inventory-items/{inventoryItemId}/decrease")]
    public static async Task<IResult> DecreaseQuantity(
    Guid inventoryItemId,
    DecreaseInventoryItemQuantityRequest request,
    IMessageBus bus,
    InventoryItemMapper mapper,
    CancellationToken cancellationToken)
    {
        var command = new DecreaseInventoryItemQuantityCommand(
            inventoryItemId,
            request.Quantity);

        var result = await bus.InvokeAsync<Result>(
            command,
            cancellationToken);

        return result.ToHttpResult();
    }
}