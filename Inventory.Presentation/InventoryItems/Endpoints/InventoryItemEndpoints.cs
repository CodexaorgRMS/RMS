using FluentResults;
using Inventory.Application.Features.InventoryItems.Commands.DecreaseQuantity;
using Inventory.Application.Features.InventoryItems.Commands.IncreaseQuantity;
using Inventory.Application.Features.InventoryItems.Commands.UpdateMinStock;
using Inventory.Presentation.Extentions;
using Inventory.Presentation.InventoryItems.Mappers;
using Inventory.Presentation.InventoryItems.Requests;
using Microsoft.AspNetCore.Http;
using Wolverine;
using Wolverine.Http;
using SharedPresentation.Extentions;
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

        var result = await bus.InvokeAsync<Result<Guid>>(
            command,
            cancellationToken);


        return result.ToCreatedResult(
            $"/api/inventory-items/{result.Value}");
    }

    [WolverinePut("/api/inventory-items/{inventoryItemId}/min-stock")]
    public static async Task<IResult> UpdateMinStock(
    Guid inventoryItemId,
    UpdateInventoryItemMinStockRequest request,
    IMessageBus bus,
    CancellationToken cancellationToken)
    {
        var command = new UpdateInventoryItemMinStockCommand(
            inventoryItemId,
            request.MinStock);

        var result =
            await bus.InvokeAsync<FluentResults.Result<UpdateInventoryItemMinStockResult>>(
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

        return Results.Ok(result.Value);
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

        var result =
       await bus.InvokeAsync<FluentResults.Result<IncreaseInventoryItemQuantityResult>>(
           command,
           cancellationToken);

        return Results.Ok(result.Value);
    }

    [WolverinePost("/api/inventory-items/{inventoryItemId}/decrease")]
    public static async Task<IResult> DecreaseQuantity(
     Guid inventoryItemId,
     DecreaseInventoryItemQuantityRequest request,
     IMessageBus bus,
     CancellationToken cancellationToken)
    {
        var command = new DecreaseInventoryItemQuantityCommand(
            inventoryItemId,
            request.Quantity);

        var result =
            await bus.InvokeAsync<FluentResults.Result<DecreaseInventoryItemQuantityResult>>(
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

        return Results.Ok(result.Value);
    }
}