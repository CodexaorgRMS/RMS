using FluentResults;
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
}