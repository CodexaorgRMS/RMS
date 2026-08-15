using Inventory.Domain.Enums;

namespace Inventory.Domain.Entities
{
	public class Category
	{
		public Guid CategoryId { get; set; }

		public string Name { get; set; } = string.Empty;

		public string Description { get; set; } = string.Empty;
		public PickingStrategy DefaultPickingStrategy { get; set; } = PickingStrategy.FEFO;

        // Category-level Expiry & Markdown Defaults
        public int ExpiryWarningDays { get; set; } = 7;
        public decimal AutoMarkdownPercentage { get; set; } = 0;

        public Guid? ParentId { get; set; }

		public Category? Parent { get; set; }

		public ICollection<Category> Children { get; set; }
			= new List<Category>();

		public ICollection<Product> Products { get; set; }
			= new List<Product>();
	}
}
