using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sales.Domain.Entities;

namespace Sales.Infrastructure.Data.Configurations
{
	public class OrderConfiguration : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.ToTable("Orders"); 

			builder.HasKey(o => o.OrderId);

			builder.Property(i => i.OrderId)
	              .ValueGeneratedOnAdd();


			builder.Property(o => o.OrderNumber)
				.IsRequired()
				.HasMaxLength(50);

			builder.Property(o => o.SubTotal).HasPrecision(18, 2);
			builder.Property(o => o.DiscountAmount).HasPrecision(18, 2);
			builder.Property(o => o.TotalAmount).HasPrecision(18, 2);
			builder.Property(o => o.PaidAmount).HasPrecision(18, 2);

			builder.HasIndex(o => o.CustomerId);


			builder.HasMany(o => o.Items)
				.WithOne(i => i.Order)
				.HasForeignKey(i => i.OrderId)
				.OnDelete(DeleteBehavior.Cascade); 
		}
	}
}
