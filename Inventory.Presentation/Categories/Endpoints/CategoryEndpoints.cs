using FluentResults;
using FluentValidation;
using Inventory.Application.Features.Categories.Commands.Delete;
using Inventory.Presentation.Categories.Mappers;
using Inventory.Presentation.Categories.Requests;
using Inventory.Presentation.Extentions;
using Microsoft.AspNetCore.Http;
using Wolverine;
using Wolverine.Http;

namespace Inventory.Presentation.Categories.Endpoints;

public static class CategoryEndpoints
{
    [WolverinePost("/api/categories")]
    public static async Task<IResult> Create(
        CreateCategoryRequest request,
        IMessageBus bus,
        CategoryMapper mapper)
    {

        var result = await bus.InvokeAsync<FluentResults.Result<Guid>>(
            mapper.MapToCommand(request));

        return result.ToCreatedResult(
            $"/api/categories/{result.Value}");
    }

    [WolverinePut("/api/categories/{categoryId}")]
	public static async Task<IResult> Update(
		Guid categoryId,
		UpdateCategoryRequest request,
		IMessageBus bus,
		CategoryMapper mapper)
	{
		if (categoryId != request.CategoryId)
			return Results.BadRequest("Category ID mismatch.");


		var result = await bus.InvokeAsync<Result>(
			mapper.MapToCommand(request));

		return result.ToHttpResult();
	}

	[WolverineDelete("/api/categories/{categoryId}")]
	public static async Task<IResult> Delete(
		Guid categoryId,
		IMessageBus bus)
	{
		var result = await bus.InvokeAsync<Result>(
			new DeleteCategoryCommand(categoryId));

		return result.ToHttpResult();
	}

	[WolverinePut("/api/categories/{categoryId}/picking-strategy")]
	public static async Task<IResult> UpdatePickingStrategy(
		Guid categoryId,
		UpdateCategoryPickingStrategyRequest request,
		IMessageBus bus,
		CategoryMapper mapper)
	{
		if (categoryId != request.CategoryId)
			return Results.BadRequest("Category ID mismatch.");
		var result = await bus.InvokeAsync<Result>(
			mapper.MapToCommand(request));
		return result.ToHttpResult();
	}

    [WolverinePut("/api/categories/{categoryId:guid}/expiry-rule")]
    public static async Task<IResult> UpdateExpiryRule(
        Guid categoryId,
        UpdateCategoryExpiryRuleRequest request,
        IMessageBus bus,
        CategoryMapper mapper,
        CancellationToken cancellationToken)
    {
        var command = mapper.MapToCommand(categoryId, request);
        var result = await bus.InvokeAsync<Result>(command, cancellationToken);

        if (result.IsFailed)
        {
            return Results.Problem(
                detail: string.Join("; ", result.Errors.Select(x => x.Message)),
                statusCode: StatusCodes.Status400BadRequest);
        }

        return Results.NoContent();
    }
}