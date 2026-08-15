using FluentResults;
using FluentValidation;
using Inventory.Application.Features.ProductBatches.Commands.ChangeStatus;
using Inventory.Application.Features.ProductBatches.Commands.CheckExpiring;
using Inventory.Presentation.Extentions;
using Inventory.Presentation.ProductBatches.Mappers;
using Inventory.Presentation.ProductBatches.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

    [WolverinePut("/api/product-batches/{batchId:guid}/status")]
    public static async Task<IResult> ChangeBatchStatus(
        Guid batchId,
        ChangeProductBatchStatusRequest request,
        IMessageBus bus,
        ProductBatchMapper mapper,
        CancellationToken cancellationToken)
    {
        var command = mapper.MapToCommand(batchId, request);

        var result = await bus.InvokeAsync<Result>(
            command,
            cancellationToken);

        if (result.IsFailed)
        {
            return Results.Problem(
                detail: string.Join("; ", result.Errors.Select(x => x.Message)),
                statusCode: StatusCodes.Status400BadRequest);
        }

        return Results.NoContent();
    }

    // Manual trigger for batch expiry evaluation (For Testing & Admin On-Demand Run) (Test)
    [WolverinePost("/api/product-batches/check-expirations")]
    public static async Task<IResult> TriggerExpiryEvaluation(
        IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var result = await bus.InvokeAsync<Result>(new CheckExpiringBatchesCommand(), cancellationToken);

        if (result.IsFailed)
        {
            return Results.Problem(
                detail: string.Join("; ", result.Errors.Select(x => x.Message)),
                statusCode: StatusCodes.Status400BadRequest);
        }

        return Results.Ok(new { message = "Batch expiry evaluation executed successfully." });
    }
}
