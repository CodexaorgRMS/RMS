using Identity.Application.Abstractions.Services;
using Microsoft.AspNetCore.Http;

namespace Identity.Infrastucture.Services
{
	public class CookieService : ICookieService
	{
		private readonly IExecutionContextAccessor _executionContextAccessor;

		public CookieService(IExecutionContextAccessor executionContextAccessor)
		{
			_executionContextAccessor = executionContextAccessor;
		}

		public string? GetRefreshToken()
		{
			return _executionContextAccessor.HttpContext?
				.Request.Cookies["refreshToken"];
		}

		public void SetRefreshToken(string token, DateTime expiresOn)
		{
			_executionContextAccessor.HttpContext!.Response.Cookies.Append(
				"refreshToken",
				token,
				new CookieOptions
				{
					HttpOnly = true,
					Secure = true,
					SameSite = SameSiteMode.Strict,
					Expires = expiresOn
				});
		}

		public void DeleteRefreshToken()
		{
			_executionContextAccessor.HttpContext!.Response.Cookies.Delete("refreshToken");
		}
	}
}
