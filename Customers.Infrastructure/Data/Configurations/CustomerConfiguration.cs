using Customers.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Customers.Infrastructure.Data.Configurations;

public class CustomerConfiguration : IEntityTypeConfiguration<Customer>
{
	public void Configure(EntityTypeBuilder<Customer> builder)
	{
		builder.ToTable("Customers");



		builder.HasKey(c => c.CustomerId);

		builder.Property(c => c.Name).IsRequired().HasMaxLength(150);
		builder.Property(c => c.Phone).IsRequired().HasMaxLength(20);
		builder.Property(c => c.TotalDebt).HasPrecision(18, 2);

		builder.HasMany(c => c.Ledgers)
			.WithOne(l => l.Customer)
			.HasForeignKey(l => l.CustomerId)
			.OnDelete(DeleteBehavior.Cascade);
	}
}
