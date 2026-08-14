using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infrastructure.Data.Configurations;

public sealed class CategoryConfiguration : IEntityTypeConfiguration<Category>
{
    public void Configure(EntityTypeBuilder<Category> builder)
    {
        builder.ToTable("Categories");

        // Primary Key
        builder.HasKey(x => x.CategoryId);

        builder.Property(x => x.CategoryId)
            .ValueGeneratedOnAdd();

        // Name
        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        // Description
        builder.Property(x => x.Description)
            .IsRequired()
            .HasMaxLength(1000);

        // Parent Category
        builder.Property(x => x.ParentId)
            .IsRequired(false);

        builder.HasOne(x => x.Parent)
            .WithMany(x => x.Children)
            .HasForeignKey(x => x.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        // Perfect Indexes
        builder.HasIndex(x => x.ParentId)
            .HasDatabaseName("IX_Category_ParentId");

        builder.HasIndex(x => x.Name)
            .HasDatabaseName("IX_Category_Name");
    }
}