using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Application.Abstractions.Shared
{
	public interface IIdentityDataContext
	{
		DbSet<RefreshToken> RefreshTokens { get; set; }
		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
