namespace Identity.Domain.Entities
{
	public class RefreshToken
	{
		public Guid Id { get; set; }
		public string Token { get; set; } = string.Empty;
		public DateTime ExpiresOn { get; set; }
		public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
		public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
		public DateTime? RevokedOn { get; set; }
		public bool IsActive => RevokedOn == null && !IsExpired;

		public Guid UserId { get; set; }
		public ApplicationUser User { get; set; } = null!;
	}


}
