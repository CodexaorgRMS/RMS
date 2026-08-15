using FluentResults;
using Inventory.Presentation.StockMovements.Mappers;
using Inventory.Presentation.StockMovements.Requests;
using Microsoft.AspNetCore.Http;
using Wolverine;
using Wolverine.Http;

namespace Inventory.Presentation.StockMovements.Endpoints;

public static class StockMovementEndpoints
{
    [WolverinePost("/api/stock-movements")]
    public static async Task<IResult> Create(
        CreateStockMovementRequest request,
        IMessageBus bus,
        StockMovementMapper mapper,
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
            $"/api/stock-movements/{result.Value}",
            result.Value);
    }
}
