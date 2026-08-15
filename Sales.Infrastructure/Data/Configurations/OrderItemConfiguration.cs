using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.Domain.Entities;

namespace Sales.Infrastructure.Data.Configurations
{
	public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
	{
		public void Configure(EntityTypeBuilder<OrderItem> builder)
		{
			builder.ToTable("OrderItems");

			builder.HasKey(i => i.OrderItemId);

			builder.Property(i => i.ProductName)
				.IsRequired()
				.HasMaxLength(250);

			builder.Property(i => i.UnitPrice).HasPrecision(18, 2);
			builder.Property(i => i.TotalPrice).HasPrecision(18, 2);


			builder.HasIndex(i => i.ProductId);
		}
	}
}
