using Customers.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Customers.Infrastructure.Data.Configurations;

public class CustomerLedgerConfiguration : IEntityTypeConfiguration<CustomerLedger>
{
	public void Configure(EntityTypeBuilder<CustomerLedger> builder)
	{
		builder.ToTable("CustomerLedgers");

		builder.Property(c => c.LedgerId).ValueGeneratedOnAdd();

		builder.HasKey(l => l.LedgerId);
		builder.Property(l => l.Amount).HasPrecision(18, 2);
		builder.Property(l => l.Type).IsRequired();
	}
}
