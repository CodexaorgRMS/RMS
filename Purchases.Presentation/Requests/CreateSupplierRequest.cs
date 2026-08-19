using System;
using System.Collections.Generic;
using System.Text;

namespace Purchases.Presentation.Requests
{
    public sealed record CreateSupplierRequest
    (
        string Name,
        string? Phone,
        string? Email,
        string? Address
    );
}
