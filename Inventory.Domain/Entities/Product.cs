using Inventory.Domain.Enums;

namespace Inventory.Domain.Entities
{
    public class Product
    {
		public Guid ProductId { get; set; }

		public string Barcode { get; set; } = string.Empty;

		public string Name { get; set; } = string.Empty;
		public string Description { get; set; } = string.Empty;
		
		public decimal SellingPrice { get; set; }

		public bool IsActive { get; set; } = true;

		public Guid CategoryId { get; set; }
		public Category Category { get; set; } = null!;
		public PickingStrategy? CustomPickingStrategy { get; set; }

		public  ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
        public  ICollection<ProductBatch> ProductBatches { get; set; } = new List<ProductBatch>();
        public  ICollection<Adjustment> Adjustments { get; set; } = new List<Adjustment>();
    }
}
