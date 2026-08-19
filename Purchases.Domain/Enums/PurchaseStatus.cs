using System;
using System.Collections.Generic;
using System.Text;

namespace Purchases.Domain.Enums
{
    public enum PurchaseStatus
    {
        Draft = 1,
        Ordered = 2,
        PartiallyReceived = 3,
        Received = 4,
        Cancelled = 5
    }
}
