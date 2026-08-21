using FluentResults;
using Microsoft.EntityFrameworkCore;
using Purchases.Application.Abstractions;
using Wolverine.Attributes;

namespace Purchases.Application.Features.Suppliers.Commands.ActivateSupplier;

[Transactional]
public static class ActivateSupplierHandler
{
    public static async Task<Result> Handle(
        ActivateSupplierCommand command,
        IPurchasesDataContext context)
    {
        var supplier = await context.Suppliers
            .FirstOrDefaultAsync(x => x.SupplierId == command.SupplierId);

        if (supplier is null)
        {
            return Result.Fail("Supplier not found.");
        }

        if (supplier.IsActive)
        {
            return Result.Ok();
        }

        supplier.IsActive = true;
        supplier.UpdatedAt = DateTime.UtcNow;

        return Result.Ok();
    }
}
