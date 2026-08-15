using Microsoft.EntityFrameworkCore;
using Sales.Application.Abstractions;
using Sales.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sales.Infrastructure.Data
{

	public class SalesDbContext:DbContext, ISalesDataContext
	{
		public SalesDbContext(DbContextOptions<SalesDbContext> options) : base(options) { }


		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderItem> OrderItems { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);


			modelBuilder.HasDefaultSchema("Sales");


			modelBuilder.ApplyConfigurationsFromAssembly(typeof(SalesDbContext).Assembly);
		}
	
	}
}
