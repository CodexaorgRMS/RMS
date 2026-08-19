using System;
using System.Collections.Generic;
using System.Text;

namespace Purchases.Application.Features.Suppliers.Commands.CreateSupplier
{
    public sealed record CreateSupplierCommand(
    string Name,
    string? Phone,
    string? Email,
    string? Address,
     string IdempotencyKey);
}
