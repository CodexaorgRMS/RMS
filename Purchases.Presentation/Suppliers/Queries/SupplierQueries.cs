using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using Purchases.Application.Abstractions;
using Purchases.Presentation.Suppliers.Dtos;
using SharedPresentation.GraphQL;
using System;
using System.Linq;

namespace Purchases.Presentation.Suppliers.Queries;

[ExtendObjectType(typeof(SharedPresentation.GraphQL.Query))]
public class SupplierQueries
{
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<SupplierDto> GetSuppliers([Service] IPurchasesDataContext context)
    {
        return context.Suppliers
            .AsNoTracking()
            .Select(s => new SupplierDto(
                s.SupplierId, s.Name, s.Phone, s.Email, s.Address, s.IsActive, s.CreatedAt));
    }

    [UseFirstOrDefault]
    public IQueryable<SupplierDto> GetSupplierById(
        Guid supplierId,
        [Service] IPurchasesDataContext context)
    {
        return context.Suppliers
            .AsNoTracking()
            .Where(s => s.SupplierId == supplierId)
            .Select(s => new SupplierDto(
                s.SupplierId, s.Name, s.Phone, s.Email, s.Address, s.IsActive, s.CreatedAt));
    }
}
