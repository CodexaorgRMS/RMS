using System;

namespace Purchases.Application.Features.Purchases.Commands.Submit;

public record SubmitPurchaseResult(Guid PurchaseOrderId, string Status);
