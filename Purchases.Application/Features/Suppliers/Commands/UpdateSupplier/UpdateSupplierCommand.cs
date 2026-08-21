using System;
using System.Collections.Generic;
using System.Text;

namespace Purchases.Application.Features.Suppliers.Commands.UpdateSupplier
{
    public sealed record UpdateSupplierCommand(
     Guid SupplierId,
     string Name,
     string? Phone,
     string? Email,
     string? Address);
}
