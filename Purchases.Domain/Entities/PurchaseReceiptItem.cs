namespace Purchases.Domain.Entities
{
    public class PurchaseReceiptItem
    {
        public Guid PurchaseReceiptItemId { get; set; }

        public Guid PurchaseReceiptId { get; set; }

        public Guid PurchaseOrderItemId { get; set; }

        public Guid ProductId { get; set; }

        public int ReceivedQuantity { get; set; }

        public DateTime? ExpiryDate { get; set; }

        public PurchaseReceipt PurchaseReceipt { get; set; } = null!;

        public PurchaseOrderItem PurchaseOrderItem { get; set; } = null!;
    }
}