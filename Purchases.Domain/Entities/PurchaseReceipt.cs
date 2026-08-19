namespace Purchases.Domain.Entities
{
    public class PurchaseReceipt
    {
        public Guid PurchaseReceiptId { get; set; }

        public Guid PurchaseOrderId { get; set; }

        public DateTime ReceivedAt { get; set; }

        public PurchaseOrder PurchaseOrder { get; set; } = null!;

        public ICollection<PurchaseReceiptItem> Items { get; set; }
            = new List<PurchaseReceiptItem>();
    }
}