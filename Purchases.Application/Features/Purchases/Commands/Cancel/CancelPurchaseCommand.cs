using System;

namespace Purchases.Application.Features.Purchases.Commands.Cancel;

public record CancelPurchaseCommand(Guid PurchaseId);
