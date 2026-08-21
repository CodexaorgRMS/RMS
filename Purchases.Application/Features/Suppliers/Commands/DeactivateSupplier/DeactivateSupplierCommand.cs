using System;
using System.Collections.Generic;
using System.Text;

namespace Purchases.Application.Features.Suppliers.Commands.DeactivateSupplier
{
    public sealed record DeactivateSupplierCommand(
    Guid SupplierId);
}
