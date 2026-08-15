using FluentResults;
using Inventory.Presentation.Adjustments.Mappers;
using Inventory.Presentation.Adjustments.Requests;
using Microsoft.AspNetCore.Http;
using SharedPresentation.Extentions;
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

        var result = await bus.InvokeAsync<Result<Guid>>(
            command,
            cancellationToken);

        return result.ToCreatedResult($"/api/adjustments/{result.Value}");
    }
}
