using FluentResults;
using FluentValidation;
using Inventory.Presentation.Extentions;
using Inventory.Presentation.ProductBatches.Mappers;
using Inventory.Presentation.ProductBatches.Requests;
using Microsoft.AspNetCore.Http;
using Wolverine;
using Wolverine.Http;

namespace Inventory.Presentation.ProductBatches.Endpoints;

public static class ProductBatchEndpoints
{
    [WolverinePost("/api/product-batches/receive")]
    public static async Task<IResult> Receive(
        ReceiveProductBatchRequest request,
        IMessageBus bus,
        ProductBatchMapper mapper,
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


        return Results.Created($"/api/product-batches/{result.Value}", result.Value);
    }
}
