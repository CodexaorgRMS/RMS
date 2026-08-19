namespace Purchases.Domain.Entities
{
    public class PurchaseOrderItem
    {
        public Guid PurchaseOrderItemId { get; set; }

        public Guid PurchaseOrderId { get; set; }

        public Guid ProductId { get; set; }

        public int OrderedQuantity { get; set; }

        public decimal UnitCost { get; set; }

        public PurchaseOrder PurchaseOrder { get; set; } = null!;

        public ICollection<PurchaseReceiptItem> ReceiptItems { get; set; }
            = new List<PurchaseReceiptItem>();
    }
}