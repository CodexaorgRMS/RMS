using System;
using System.Collections.Generic;

namespace Inventory.Domain.Entities
{
    public class Product
    {
        public Guid ProductId { get; set; }
        
        public Guid CategoryId { get; set; }
        public Category Category { get; set; }
		public string Name { get; set; }
        public string Description { get; set; }
     
		public  ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
        public  ICollection<StockMovement> StockMovements { get; set; } = new List<StockMovement>();
        public  ICollection<ProductBatch> ProductBatches { get; set; } = new List<ProductBatch>();
        public  ICollection<Adjustment> Adjustments { get; set; } = new List<Adjustment>();
    }
}
