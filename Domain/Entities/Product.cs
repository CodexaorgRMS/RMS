namespace Domain.Entities
{
	public class Product
		{
			public Guid Id { get; set; }

			public string Name { get; set; } = string.Empty;

			public string Barcode { get; set; } = string.Empty;

			public decimal SalePrice { get; set; }

			public decimal CostPrice { get; set; }

			public decimal StockQuantity { get; set; }

			public bool IsWeighable { get; set; } = false;


			// public Guid CategoryId { get; set; }
			// public Category Category { get; set; } = null!;
		}
}
