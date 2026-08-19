using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Purchases.Domain.Entities;

namespace Purchases.Infrastructure.Data.Configurations;

public sealed class IdempotencyRecordConfiguration
    : IEntityTypeConfiguration<IdempotencyRecord>
{
    public void Configure(EntityTypeBuilder<IdempotencyRecord> builder)
    {
        builder.ToTable("IdempotencyRecords");

        builder.HasKey(x => x.IdempotencyRecordId);

        builder.Property(x => x.IdempotencyRecordId)
            .ValueGeneratedNever();

        builder.Property(x => x.IdempotencyKey)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Operation)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.ResourceId)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        builder.HasIndex(x => new
        {
            x.Operation,
            x.IdempotencyKey
        })
        .IsUnique();
    }
}