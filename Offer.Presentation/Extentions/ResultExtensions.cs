using FluentResults;
using Microsoft.AspNetCore.Http;
using SharedPresentation.Extentions;

namespace Offers.Presentation.Extentions;

public static class ResultExtensions
{
    public static IResult ToOkResult<T>(this Result<T> result)
    {
        if (result.IsSuccess)
        {
            return Results.Ok(result.Value);
        }

        return Results.Problem(
            detail: string.Join("; ", result.Errors.Select(x => x.Message)),
            statusCode: StatusCodes.Status400BadRequest
        );
    }
}
