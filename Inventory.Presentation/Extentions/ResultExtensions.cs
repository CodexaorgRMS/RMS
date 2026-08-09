using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Presentation.Extentions
{
	using FluentResults;
	using Microsoft.AspNetCore.Http;
	public static class ResultExtensions
	{
		public static IResult ToHttpResult(this Result result)
		{
			if (result.IsSuccess)
				return Results.NoContent();

			return Results.Problem(
				detail: string.Join("; ", result.Errors.Select(x => x.Message)),
				statusCode: StatusCodes.Status400BadRequest);
		}

		public static IResult ToCreatedResult<T>(
			this Result<T> result,
			string location)
		{
			if (result.IsSuccess)
				return Results.Created(location, result.Value);

			return Results.Problem(
				detail: string.Join("; ", result.Errors.Select(x => x.Message)),
				statusCode: StatusCodes.Status400BadRequest);
		}
	}
}
