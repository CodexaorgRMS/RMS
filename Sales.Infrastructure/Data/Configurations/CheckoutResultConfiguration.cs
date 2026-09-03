using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.Domain.Entities;

namespace Sales.Infrastructure.Data.Configurations;

/// <summary>
/// EF Core configuration for the <see cref="CheckoutResult"/> entity.
/// </summary>
public class CheckoutResultConfiguration : IEntityTypeConfiguration<CheckoutResult>
{
	public void Configure(EntityTypeBuilder<CheckoutResult> builder)
	{
		builder.ToTable("CheckoutResults");

		builder.HasKey(r => r.CheckoutResultId);
		builder.Property(r => r.CheckoutResultId).ValueGeneratedOnAdd();

		builder.HasIndex(r => r.SagaId).IsUnique();

		builder.Property(r => r.OrderNumber).HasMaxLength(50).IsRequired();
		builder.Property(r => r.ErrorMessage).HasMaxLength(2000);
	}
}
