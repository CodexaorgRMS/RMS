using FluentResults;
using Identity.Application.Abstractions.Services;
using Identity.Application.Features.Auth.Common;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Wolverine.Attributes;

namespace Identity.Application.Features.Auth.RefreshTokens;

[Transactional]
public static class RefreshTokenCommandHandler
{
	public static async Task<Result<AuthResponse>> Handle(
		RefreshTokenCommand command,
		UserManager<ApplicationUser> userManager,
		ITokenService tokenService,
		ICookieService cookieService,
		CancellationToken cancellationToken)
	{
		var refreshToken = cookieService.GetRefreshToken();

		if (string.IsNullOrWhiteSpace(refreshToken))
		{
			return Result.Fail(
				new Error("Refresh token is missing.")
					.WithMetadata("ErrorCode", "Auth.InvalidToken"));
		}

		var user = await tokenService.GetUserByRefreshTokenAsync(refreshToken);

		if (user is null)
		{
			return Result.Fail(
				new Error("Invalid refresh token.")
					.WithMetadata("ErrorCode", "Auth.InvalidToken"));
		}

		var currentRefreshToken =
			user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken);

		if (currentRefreshToken is null)
		{
			return Result.Fail(
				new Error("Refresh token not found.")
					.WithMetadata("ErrorCode", "Auth.InvalidToken"));
		}

		if (!currentRefreshToken.IsActive)
		{
			return Result.Fail(
				new Error("Refresh token has expired or was revoked.")
					.WithMetadata("ErrorCode", "Auth.InvalidToken"));
		}

		currentRefreshToken.RevokedOn = DateTime.UtcNow;

		var newRefreshToken =
			tokenService.GenerateRefreshToken(user.Id);

		user.RefreshTokens.Add(newRefreshToken);

		var accessToken =
			tokenService.GenerateAccessToken(user);

		var result = await userManager.UpdateAsync(user);

		if (!result.Succeeded)
		{
			return Result.Fail(
				new Error("Unable to refresh token."));
		}

		cookieService.SetRefreshToken(
			newRefreshToken.Token,
			newRefreshToken.ExpiresOn);

		return Result.Ok(
			new AuthResponse(
				accessToken,
				newRefreshToken.ExpiresOn));
	}
}