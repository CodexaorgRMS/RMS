using FluentResults;
using HotChocolate.Authorization;
using Identity.Application.Features.Auth.Common;
using Identity.Application.Features.Auth.RefreshTokens;
using Identity.Presentation.Mapping;
using Identity.Presentation.Requests;
using Microsoft.AspNetCore.Http;
using Wolverine;
using Wolverine.Http;

namespace Identity.Presentation.Endpoints
{
	public static class AuthEndpoints
	{
		[WolverinePost("/api/auth/login")]
		public static async Task<IResult> Handle(LoginRequest request, IMessageBus bus)
		{
			var mapper = new AuthMapper();
			var command = mapper.MapLogin(request);
			var result = await bus.InvokeAsync<Result<AuthResponse>>(command);

			if (result.IsFailed)
			{
				return CreateProblemDetails(result.Errors.First());
			}

			return Results.Ok(result.Value);
		}

		[WolverinePost("/api/auth/forget-password")]
		public static async Task<IResult> Handle(ForgetPasswordRequest request, IMessageBus bus)
		{
			var mapper = new AuthMapper();
			var command = mapper.MapForgetPassword(request);
			var result = await bus.InvokeAsync<Result>(command);

			if (result.IsFailed)
			{
				return CreateProblemDetails(result.Errors.First());
			}

			return Results.Ok(new { message = "If the email is registered, an OTP has been sent." });
		}

		[WolverinePost("/api/auth/reset-password")]
		public static async Task<IResult> Handle(ResetPasswordRequest request, IMessageBus bus)
		{
			var mapper = new AuthMapper();
			var command = mapper.MapResetPassword(request);
			var result = await bus.InvokeAsync<Result>(command);

			if (result.IsFailed)
			{
				return CreateProblemDetails(result.Errors.First());
			}

			return Results.Ok(new { message = "Password reset successfully." });
		}

		[Authorize]
		[WolverinePost("/api/auth/change-password")]
		public static async Task<IResult> Handle(ChangePasswordRequest request, IMessageBus bus)
		{
			var mapper = new AuthMapper();
			var command = mapper.MapChangePassword(request);
			var result = await bus.InvokeAsync<Result>(command);

			if (result.IsFailed)
			{
				return CreateProblemDetails(result.Errors.First());
			}

			return Results.Ok(new { message = "Password changed successfully." });
		}

		[WolverinePost("/api/auth/refresh-token")]
		public static async Task<IResult> Handle(IMessageBus bus)
		{
			var command = new RefreshTokenCommand();
			var result = await bus.InvokeAsync<Result<AuthResponse>>(command);

			if (result.IsFailed)
			{
				return CreateProblemDetails(result.Errors.First());
			}

			return Results.Ok(result.Value);
		}

		private static IResult CreateProblemDetails(IError error)
		{
			var errorCode = error.Metadata.TryGetValue("ErrorCode", out var code) ? code : "BadRequest";
			return Results.Problem(
				statusCode: StatusCodes.Status400BadRequest,
				title: "Authentication Failed",
				detail: error.Message,
				extensions: new Dictionary<string, object?> { { "errorCode", errorCode } }
			);
		}

		[WolverinePost("/api/auth/verify-email")]
		public static async Task<IResult> Handle(VerifyUserEmailRequest request, IMessageBus bus)
		{
			var mapper = new AuthMapper();
			var command = mapper.MapVerifyEmail(request);
			var result = await bus.InvokeAsync<Result>(command);

			if (result.IsFailed)
			{
				return CreateProblemDetails(result.Errors.First());
			}

			return Results.Ok(new { message = "Email verified successfully." });
		}
	}

}
