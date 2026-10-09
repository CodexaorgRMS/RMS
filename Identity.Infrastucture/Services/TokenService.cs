using Identity.Application.Abstractions.Services;
using Identity.Domain.Entities;
using Identity.Infrastucture.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Identity.Infrastucture.Services
{
	public class TokenService : ITokenService
	{
		private readonly IConfiguration _configuration;
		private readonly AppIdentityDbContext _dbContext;

		public TokenService(IConfiguration configuration, AppIdentityDbContext dbContext)
		{
			_configuration = configuration;
			_dbContext = dbContext;
		}

		public string GenerateAccessToken(ApplicationUser user)
		{
			var claims = new List<Claim>
		{
			new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
			new Claim(JwtRegisteredClaimNames.Email, user.Email ?? string.Empty),
			new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
		};

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
				_configuration["Jwt:Key"] ?? "super_secret_fallback_key_that_is_long_enough_12345!"));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				issuer: _configuration["Jwt:Issuer"] ?? "EduSystem",
				audience: _configuration["Jwt:Audience"] ?? "EduSystemUsers",
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(int.Parse(_configuration["Jwt:DurationInMinutes"] ?? "60")),
				signingCredentials: creds
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}

		public RefreshToken GenerateRefreshToken(Guid userId)
		{
			var randomNumber = new byte[32];
			using var rng = RandomNumberGenerator.Create();
			rng.GetBytes(randomNumber);

			return new RefreshToken
			{
				Id = Guid.NewGuid(),
				Token = Convert.ToBase64String(randomNumber),
				ExpiresOn = DateTime.UtcNow.AddDays(int.Parse(_configuration["Jwt:RefreshTokenExpirationInDays"] ?? "7")),
				CreatedOn = DateTime.UtcNow,
				UserId = userId
			};
		}

		public async Task<ApplicationUser?> GetUserByRefreshTokenAsync(string refreshToken)
		{
			return await _dbContext.Users
				.Include(u => u.RefreshTokens)
				.SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken));
		}
	}
}
