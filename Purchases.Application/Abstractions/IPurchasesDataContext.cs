using Microsoft.EntityFrameworkCore;
using Purchases.Domain.Entities;

namespace Purchases.Application.Abstractions
{
    public interface IPurchasesDataContext
    {
        DbSet<Supplier> Suppliers { get; }
    }
}
