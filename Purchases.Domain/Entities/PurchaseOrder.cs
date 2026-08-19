using Purchases.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Purchases.Domain.Entities
{
    public class PurchaseOrder
    {
        public Guid PurchaseOrderId { get; set; }

        public Guid SupplierId { get; set; }

        public PurchaseStatus Status { get; set; } = PurchaseStatus.Draft;

        public decimal TotalAmount { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? SubmittedAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        public DateTime? CancelledAt { get; set; }

        public Supplier Supplier { get; set; } = null!;

        public ICollection<PurchaseOrderItem> Items { get; set; }
            = new List<PurchaseOrderItem>();

        public ICollection<PurchaseReceipt> Receipts { get; set; }
            = new List<PurchaseReceipt>();
    }
}
