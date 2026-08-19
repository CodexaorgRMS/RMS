using Microsoft.EntityFrameworkCore;
using Purchases.Application.Abstractions;
using Purchases.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Purchases.Infrastructure.Data
{
    public class PurchasesDbContext : DbContext, IPurchasesDataContext
    {
        public PurchasesDbContext(DbContextOptions<PurchasesDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(PurchasesDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public DbSet<PurchaseReceipt> PurchaseReceipts { get; set; }
        public DbSet<PurchaseReceiptItem> PurchaseReceiptItems { get; set; }
        public DbSet<IdempotencyRecord> IdempotencyRecords { get; set; }
    }
}
