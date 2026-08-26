using Finance.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Finance.Application.Abstractions;

public interface IFinanceDataContext
{
    DbSet<Shift> Shifts { get; }
    DbSet<CashMovement> CashMovements { get; }
    DbSet<ExpenseCategory> ExpenseCategories { get; }
    DbSet<Expense> Expenses { get; }
    DbSet<FinancialObligation> FinancialObligations { get; }
    DbSet<ObligationSettlement> ObligationSettlements { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
