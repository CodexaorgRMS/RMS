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
            .ValueGeneratedOnAdd();

        // CategoryId
        builder.Property(x => x.CategoryId)
            .IsRequired();

        // Barcode
        builder.Property(x => x.Barcode)
            .HasMaxLength(100);

        // Name
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        // Description
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(1000);

        // SellingPrice
        builder.Property(x => x.SellingPrice)
            .HasPrecision(18, 2)
            .IsRequired();

        // IsActive
        builder.Property(x => x.IsActive)
            .IsRequired();

        // Product -> Category
        builder.HasOne(x => x.Category)
            .WithMany(x => x.Products)
            .HasForeignKey(x => x.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Perfect Indexes
        builder.HasIndex(x => x.CategoryId)
            .HasDatabaseName("IX_Product_CategoryId");

        builder.HasIndex(x => x.Name)
            .HasDatabaseName("IX_Product_Name");

        builder.HasIndex(x => x.Barcode)
            .HasDatabaseName("IX_Product_Barcode");

        builder.HasIndex(x => x.IsActive)
            .HasDatabaseName("IX_Product_IsActive");
    }
}