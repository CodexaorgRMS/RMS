using Identity.Domain.Entities;

namespace Identity.Application.Abstractions.Services
{
	public interface ITokenService
	{
		string GenerateAccessToken(ApplicationUser user);
		RefreshToken GenerateRefreshToken(Guid userId);
		Task<ApplicationUser?> GetUserByRefreshTokenAsync(string refreshToken);
	}
}
