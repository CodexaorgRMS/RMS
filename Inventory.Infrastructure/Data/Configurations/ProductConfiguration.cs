using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Data.Configurations;

public sealed class ProductConfiguration : IEntityTypeConfiguration<Product>
{
	public void Configure(EntityTypeBuilder<Product> builder)
	{
		builder.ToTable("Products");

		// Primary Key
		builder.HasKey(x => x.ProductId);

		builder.Property(x => x.ProductId)
			.ValueGeneratedNever();

		// CategoryId
		builder.Property(x => x.CategoryId)
			.IsRequired();

		// Name
		builder.Property(x => x.Name)
			.IsRequired()
			.HasMaxLength(200);

		// Description
		builder.Property(x => x.Description)
			.IsRequired()
			.HasMaxLength(1000);

		// Product -> Category
		builder.HasOne(x => x.Category)
			.WithMany(x => x.Products)
			.HasForeignKey(x => x.CategoryId)
			.OnDelete(DeleteBehavior.Restrict);

		// Indexes
		builder.HasIndex(x => x.CategoryId);

		builder.HasIndex(x => x.Name);
	}
}