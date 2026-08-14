using FluentResults;
using FluentValidation;
using Inventory.Presentation.Adjustments.Mappers;
using Inventory.Presentation.Adjustments.Requests;
using Inventory.Presentation.Extentions;
using Microsoft.AspNetCore.Http;
using Wolverine;
using Wolverine.Http;

namespace Inventory.Presentation.Adjustments.Endpoints;

public static class AdjustmentEndpoints
{
    [WolverinePost("/api/adjustments")]
    public static async Task<IResult> Create(
        CreateAdjustmentRequest request,
        IMessageBus bus,
        AdjustmentMapper mapper,
        CancellationToken cancellationToken)
    {
        var command = mapper.MapToCommand(request);

        var result = await bus.InvokeAsync<FluentResults.Result<Guid>>(
            command,
            cancellationToken);

        if (result.IsFailed)
        {
            return Results.Problem(
                detail: string.Join("; ", result.Errors.Select(x => x.Message)),
                statusCode: StatusCodes.Status400BadRequest);
        }

        return Results.Created($"/api/adjustments/{result.Value}", result.Value);
    }
}
