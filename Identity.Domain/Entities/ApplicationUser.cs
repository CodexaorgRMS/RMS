using Microsoft.AspNetCore.Identity;
namespace Identity.Domain.Entities
{
	public class ApplicationUser : IdentityUser<Guid>
	{
		public string? Otp { get; set; }
		public DateTime? OtpExpiration { get; set; }

		public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

		public string FirstName { get; set; } = string.Empty;
		public string LastName { get; set; } = string.Empty;
		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
		public bool IsActive { get; set; } = true;
	}


}
