using FluentResults;
using Inventory.Application.Features.Products.Commands.Delete;
using Inventory.Presentation.Extentions;
using Inventory.Presentation.Products.Mappers;
using Inventory.Presentation.Products.Requests;
using Microsoft.AspNetCore.Http;
using Wolverine;
using Wolverine.Http;
using SharedPresentation.Extentions;
namespace Inventory.Presentation.Products.Endpoints;

public static class ProductEndpoints
{
	[WolverinePost("/api/products")]
	public static async Task<IResult> Create(
		CreateProductRequest request,
		IMessageBus bus,
		ProductMapper mapper)
	{
		var command = mapper.MapToCommand(request);

		var result = await bus.InvokeAsync<FluentResults.Result<Guid>>(command);

		return result.ToCreatedResult($"/api/products/{result.Value}");
	}

	[WolverinePut("/api/products/{productId}")]
	public static async Task<IResult> Update(
		Guid productId,
		UpdateProductRequest request,
		IMessageBus bus,
		ProductMapper mapper)
	{
		if (productId != request.ProductId)
			return Results.BadRequest("Product ID mismatch.");


		var command = mapper.MapToCommand(request);

		var result = await bus.InvokeAsync<Result>(command);

		return result.ToHttpResult();
	}

	[WolverineDelete("/api/products/{productId}")]
	public static async Task<IResult> Delete(
		Guid productId,
		IMessageBus bus)
	{
		var result = await bus.InvokeAsync<Result>(
			new DeleteProductCommand(productId));

		return result.ToHttpResult();
	}

	[WolverinePut("/api/products/{productId}/picking-strategy")]
	public static async Task<IResult> UpdatePickingStrategy(
		Guid productId,
		UpdateProductPickingStrategyRequest request,
		IMessageBus bus,
		ProductMapper mapper)
	{
		if (productId != request.ProductId)
			return Results.BadRequest("Product ID mismatch.");
		var command = mapper.MapToCommand(request);
		var result = await bus.InvokeAsync<Result>(command);
		return result.ToHttpResult();
	}

    [WolverinePut("/api/products/{productId:guid}/expiry-rule")]
    public static async Task<IResult> UpdateExpiryRule(
        Guid productId,
        UpdateProductExpiryRuleRequest request,
        IMessageBus bus,
        ProductMapper mapper,
        CancellationToken cancellationToken)
    {
        var command = mapper.MapToCommand(productId, request);
        var result = await bus.InvokeAsync<Result>(command, cancellationToken);

		return result.ToHttpResult();
	}
}
