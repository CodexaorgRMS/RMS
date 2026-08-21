using System;

namespace Purchases.Application.Features.Purchases.Commands.Cancel;

public record CancelPurchaseResult(Guid PurchaseOrderId, string Status);
