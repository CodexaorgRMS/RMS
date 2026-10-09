using FluentResults;
using Identity.Application.Abstractions.Repositories;
using Identity.Application.Abstractions.Services;
using Identity.Application.Features.Auth.Common;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Wolverine.Attributes;

namespace Identity.Application.Features.Auth.Login;

[Transactional]
public static class LoginCommandHandler
{
	public static async Task<Result<AuthResponse>> Handle(
		LoginCommand command,
		UserManager<ApplicationUser> userManager,
		IGenericRepository<RefreshToken> repository,
		ITokenService tokenService,
		ICookieService cookieService,
		CancellationToken cancellationToken)
	{
		var user = await userManager.FindByEmailAsync(command.Email);

		if (user is null)
		{
			return Result.Fail(
				new Error("Invalid credentials.")
					.WithMetadata("ErrorCode", "Auth.InvalidCredentials"));
		}

		var passwordValid =
			await userManager.CheckPasswordAsync(user, command.Password);

		if (!passwordValid)
		{
			return Result.Fail(
				new Error("Invalid credentials.")
					.WithMetadata("ErrorCode", "Auth.InvalidCredentials"));
		}

		var accessToken = tokenService.GenerateAccessToken(user);

		var refreshToken = tokenService.GenerateRefreshToken(user.Id);

	
		await repository.AddAsync(refreshToken);



		cookieService.SetRefreshToken(
			refreshToken.Token,
			refreshToken.ExpiresOn);

		return Result.Ok(
			new AuthResponse(
				accessToken,
				refreshToken.ExpiresOn));
	}
}