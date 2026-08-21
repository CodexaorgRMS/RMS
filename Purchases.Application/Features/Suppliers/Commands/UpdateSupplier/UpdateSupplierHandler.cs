using FluentResults;
using Microsoft.EntityFrameworkCore;
using Purchases.Application.Abstractions;
using Wolverine.Attributes;

namespace Purchases.Application.Features.Suppliers.Commands.UpdateSupplier;

[Transactional]
public static class UpdateSupplierHandler
{
    public static async Task<Result> Handle(
        UpdateSupplierCommand command,
        IPurchasesDataContext context)
    {
        var supplier = await context.Suppliers
            .FirstOrDefaultAsync(x =>
                x.SupplierId == command.SupplierId);

        if (supplier is null)
        {
            return Result.Fail("Supplier not found.");
        }

        supplier.Name = command.Name.Trim();
        supplier.Phone = command.Phone?.Trim();
        supplier.Email = command.Email?.Trim();
        supplier.Address = command.Address?.Trim();
        supplier.UpdatedAt = DateTime.UtcNow;

        return Result.Ok();
    }
}