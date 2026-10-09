namespace Identity.Application.Abstractions.Services
{
	public interface ICookieService
	{
		string? GetRefreshToken();
		void SetRefreshToken(string token, DateTime expiresOn);
		void DeleteRefreshToken();
	}
}
