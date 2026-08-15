using System.Diagnostics;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.Extensions.Logging;
using SharedKernel.Exeptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SharedInfrastructure.ExeptionHandling
{
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
		
			if (httpContext.Response.HasStarted)
			{
				_logger.LogWarning("The response has already started, the exception handler will not be executed.");
				return false;
			}

			var (statusCode, title) = MapException(exception);

			ProblemDetails problemDetails = exception switch
			{
				CommandValidationException validationException => new ValidationProblemDetails(validationException.Errors)
				{
					Status = statusCode,
					Title = title,
					Detail = "One or more validation errors occurred."
				},
				_ => new ProblemDetails
				{
					Status = statusCode,
					Title = title,
					Detail = statusCode == StatusCodes.Status500InternalServerError
						? "An unexpected error occurred in the system."
						: exception.Message
				}
			};

			problemDetails.Instance = httpContext.Request.Path;
			problemDetails.Extensions["traceId"] = Activity.Current?.Id ?? httpContext.TraceIdentifier;

			LogException(exception, statusCode);

			httpContext.Response.StatusCode = statusCode;

			await httpContext.Response.WriteAsJsonAsync(problemDetails, problemDetails.GetType(), cancellationToken: cancellationToken);

			return true;
		}

		private static (int StatusCode, string Title) MapException(Exception exception) => exception switch
		{
			CommandValidationException => (StatusCodes.Status400BadRequest, "Validation Error"),
			NotFoundException => (StatusCodes.Status404NotFound, "Resource Not Found"),
			ConflictException => (StatusCodes.Status409Conflict, "Conflict"),
			ForbiddenException => (StatusCodes.Status403Forbidden, "Forbidden"),
			UnauthorizedAccessException => (StatusCodes.Status401Unauthorized, "Unauthorized"),
			_ => (StatusCodes.Status500InternalServerError, "Internal Server Error")
		};

		private void LogException(Exception exception, int statusCode)
		{
			if (statusCode == StatusCodes.Status500InternalServerError)
			{
				_logger.LogError(exception, "An unexpected error occurred: {Message}", exception.Message);
			}
			else
			{
				_logger.LogWarning(exception, "A handled exception occurred with status {StatusCode}: {Message}", statusCode, exception.Message);
			}
		}
	}
}
