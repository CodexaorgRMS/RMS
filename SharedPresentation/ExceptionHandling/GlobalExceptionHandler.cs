// BuildingBlocks/SharedPresentation/ExceptionHandling/GlobalExceptionHandler.cs
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SharedKernel.Exeptions;

namespace SharedPresentation.ExceptionHandling;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
	private readonly ILogger<GlobalExceptionHandler> _logger;

	public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
	{
		_logger = logger;
	}

	public async ValueTask<bool> TryHandleAsync(
		HttpContext httpContext,
		Exception exception,
		CancellationToken cancellationToken)
	{
		switch (exception)
		{
			case CommandValidationException validationException:
				await WriteValidationProblem(httpContext, validationException, cancellationToken);
				return true;

			default:
				_logger.LogError(exception, "Unhandled exception occurred while processing {Path}",
					httpContext.Request.Path);
				await WriteUnexpectedProblem(httpContext, cancellationToken);
				return true;
		}
	}

	private static async Task WriteValidationProblem(
		HttpContext httpContext,
		CommandValidationException exception,
		CancellationToken cancellationToken)
	{
		var errors = exception.Failures
			.GroupBy(f => f.PropertyName)
			.ToDictionary(
				g => g.Key,
				g => g.Select(f => f.ErrorMessage).ToArray());

		var problemDetails = new ValidationProblemDetails(errors)
		{
			Status = StatusCodes.Status400BadRequest,
			Title = "Validation failed",
			Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
			Instance = httpContext.Request.Path
		};

		httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
		httpContext.Response.ContentType = "application/problem+json";
		await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
	}

	private static async Task WriteUnexpectedProblem(
		HttpContext httpContext,
		CancellationToken cancellationToken)
	{
		var problemDetails = new ProblemDetails
		{
			Status = StatusCodes.Status500InternalServerError,
			Title = "An unexpected error occurred",
			Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
			Instance = httpContext.Request.Path
		};

		httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
		httpContext.Response.ContentType = "application/problem+json";
		await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
	}
}