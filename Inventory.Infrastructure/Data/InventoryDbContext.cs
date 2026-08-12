using Inventory.Application.Abstractions;
using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infrastructure.Data
{
	public class InventoryDbContext :DbContext, IInventoryDataContext
	{
		public InventoryDbContext(DbContextOptions<InventoryDbContext>   options)
			: base(options)
		{
		}

		public DbSet<Product> Products { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<InventoryItem> InventoryItems { get; set; }
		public DbSet<StockMovement> StockMovements { get; set; }
		public DbSet<ProductBatch> ProductBatches { get; set; }
		public DbSet<Adjustment> Adjustments { get; set; }


		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.ApplyConfigurationsFromAssembly(typeof(InventoryDbContext).Assembly);
		}

	}
}
