using System;
using System.Collections.Generic;
using System.Text;

namespace Purchases.Presentation.Requests
{
    public sealed record UpdateSupplierRequest(
     string Name,
     string? Phone,
     string? Email,
     string? Address);
}
