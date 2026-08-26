using FluentResults;
using Microsoft.AspNetCore.Http;
using Offers.Application.Features.Offers.Commands.Delete;
using Offers.Domain.Models;
using Offers.Presentation.Extentions;
using Offers.Presentation.Offers.Dtos;
using Offers.Presentation.Offers.Mappers;
using Offers.Presentation.Offers.Requests;
using SharedPresentation.Extentions;
using Wolverine;
using Wolverine.Http;

namespace Offers.Presentation.Offers.Endpoints;

public static class OfferEndpoints
{
    [WolverinePost("/api/offers/percentage")]
    public static async Task<IResult> CreatePercentageOffer(
        CreatePercentageOfferRequest request,
        IMessageBus bus,
        OfferMapper mapper,
        CancellationToken cancellationToken)
    {
        var command = mapper.MapToCommand(request);
        var result = await bus.InvokeAsync<Result<Guid>>(command, cancellationToken);

        return result.ToCreatedResult($"/api/offers/{result.ValueOrDefault}");
    }

    [WolverinePost("/api/offers/fixed")]
    public static async Task<IResult> CreateFixedOffer(
        CreateFixedOfferRequest request,
        IMessageBus bus,
        OfferMapper mapper,
        CancellationToken cancellationToken)
    {
        var command = mapper.MapToCommand(request);
        var result = await bus.InvokeAsync<Result<Guid>>(command, cancellationToken);

        return result.ToCreatedResult($"/api/offers/{result.ValueOrDefault}");
    }

    [WolverinePost("/api/offers/bogo")]
    public static async Task<IResult> CreateBogoOffer(
        CreateBogoOfferRequest request,
        IMessageBus bus,
        OfferMapper mapper,
        CancellationToken cancellationToken)
    {
        var command = mapper.MapToCommand(request);
        var result = await bus.InvokeAsync<Result<Guid>>(command, cancellationToken);

        return result.ToCreatedResult($"/api/offers/{result.ValueOrDefault}");
    }

    [WolverinePost("/api/offers/bundle")]
    public static async Task<IResult> CreateBundleOffer(
        CreateBundleOfferRequest request,
        IMessageBus bus,
        OfferMapper mapper,
        CancellationToken cancellationToken)
    {
        var command = mapper.MapToCommand(request);
        var result = await bus.InvokeAsync<Result<Guid>>(command, cancellationToken);

        return result.ToCreatedResult($"/api/offers/{result.ValueOrDefault}");
    }

    [WolverinePut("/api/offers/{offerId:guid}/status")]
    public static async Task<IResult> ToggleOfferStatus(
        Guid offerId,
        ToggleOfferStatusRequest request,
        IMessageBus bus,
        OfferMapper mapper,
        CancellationToken cancellationToken)
    {
        var command = mapper.MapToCommand(offerId, request);
        var result = await bus.InvokeAsync<Result>(command, cancellationToken);

        return result.ToHttpResult();
    }

    [WolverineDelete("/api/offers/{offerId:guid}")]
    public static async Task<IResult> DeleteOffer(
        Guid offerId,
        IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var command = new DeleteOfferCommand(offerId);
        var result = await bus.InvokeAsync<Result>(command, cancellationToken);

        return result.ToHttpResult();
    }

    [WolverinePost("/api/offers/evaluate-cart")]
    public static async Task<IResult> EvaluateCart(
        EvaluateCartRequest request,
        IMessageBus bus,
        OfferMapper mapper,
        CancellationToken cancellationToken)
    {
        var command = mapper.MapToCommand(request);
        var result = await bus.InvokeAsync<Result<DiscountEvaluationResult>>(command, cancellationToken);

        if (result.IsFailed)
        {
            return Results.Problem(
                detail: string.Join("; ", result.Errors.Select(x => x.Message)),
                statusCode: StatusCodes.Status400BadRequest
            );
        }

        var dto = mapper.MapToDto(result.Value);
        return Results.Ok(dto);
    }
}
