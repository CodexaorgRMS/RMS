using System;
using System.Collections.Generic;
using System.Text;

namespace Purchases.Application.Features.Suppliers.Commands.ActivateSupplier
{
    public sealed record ActivateSupplierCommand(
    Guid SupplierId);
}
