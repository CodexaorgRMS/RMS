using System;
using System.Collections.Generic;
using System.Text;

namespace SharedContracts.Inventory.Events;

// Published when a product batch is nearing its expiration date
public sealed record BatchNearingExpiryIntegrationEvent(
    Guid BatchId,
    Guid ProductId,
    string ProductName,
    string Barcode,
    DateTime ExpiryDate,
    int RemainingDays,
    int CurrentQuantity,
    decimal SuggestedDiscountPercentage,
    DateTime OccurredAt);