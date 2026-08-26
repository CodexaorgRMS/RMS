using Finance.Application.Abstractions;
using Finance.Domain.Enums;
using Finance.Presentation.Dtos;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Microsoft.EntityFrameworkCore;
using SharedPresentation.GraphQL;

namespace Finance.Presentation.Queries;

[ExtendObjectType(typeof(SharedPresentation.GraphQL.Query))]
public sealed class FinanceQueries
{
    /// <summary>
    /// Gets a paged, filterable, and sortable list of all shifts.
    /// </summary>
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<ShiftDto> GetShifts([Service] IFinanceDataContext context)
    {
        return context.Shifts
            .AsNoTracking()
            .Select(s => new ShiftDto
            {
                ShiftId = s.ShiftId,
                CashierId = s.CashierId,
                OpenedAt = s.OpenedAt,
                ClosedAt = s.ClosedAt,
                OpeningFloat = s.OpeningFloat,
                ActualCashEnd = s.ActualCashEnd,
                ExpectedCashEnd = s.ExpectedCashEnd,
                Variance = s.Variance,
                Status = s.Status.ToString(),
                Notes = s.Notes
            });
    }

    /// <summary>
    /// Gets a single shift by ID.
    /// </summary>
    [UseFirstOrDefault]
    public IQueryable<ShiftDto> GetShiftById(
        Guid shiftId,
        [Service] IFinanceDataContext context)
    {
        return context.Shifts
            .AsNoTracking()
            .Where(s => s.ShiftId == shiftId)
            .Select(s => new ShiftDto
            {
                ShiftId = s.ShiftId,
                CashierId = s.CashierId,
                OpenedAt = s.OpenedAt,
                ClosedAt = s.ClosedAt,
                OpeningFloat = s.OpeningFloat,
                ActualCashEnd = s.ActualCashEnd,
                ExpectedCashEnd = s.ExpectedCashEnd,
                Variance = s.Variance,
                Status = s.Status.ToString(),
                Notes = s.Notes
            });
    }

    /// <summary>
    /// Gets the current active open shift for a cashier.
    /// </summary>
    [UseFirstOrDefault]
    public IQueryable<ShiftDto> GetCurrentOpenShift(
        Guid cashierId,
        [Service] IFinanceDataContext context)
    {
        return context.Shifts
            .AsNoTracking()
            .Where(s => s.CashierId == cashierId && s.Status == ShiftStatus.Open)
            .Select(s => new ShiftDto
            {
                ShiftId = s.ShiftId,
                CashierId = s.CashierId,
                OpenedAt = s.OpenedAt,
                ClosedAt = s.ClosedAt,
                OpeningFloat = s.OpeningFloat,
                ActualCashEnd = s.ActualCashEnd,
                ExpectedCashEnd = s.ExpectedCashEnd,
                Variance = s.Variance,
                Status = s.Status.ToString(),
                Notes = s.Notes
            });
    }

    /// <summary>
    /// Gets a paged, filterable, and sortable list of cash movements across all sources.
    /// </summary>
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<CashMovementDto> GetCashMovements([Service] IFinanceDataContext context)
    {
        return context.CashMovements
            .AsNoTracking()
            .Select(m => new CashMovementDto
            {
                MovementId = m.MovementId,
                ShiftId = m.ShiftId,
                Amount = m.Amount,
                Type = m.Type.ToString(),
                Description = m.Description,
                Source = m.Source.ToString(),
                ReferenceId = m.ReferenceId,
                CreatedAt = m.CreatedAt,
                CreatedBy = m.CreatedBy
            });
    }

    /// <summary>
    /// Gets all expense categories.
    /// </summary>
    [UseFiltering]
    [UseSorting]
    public IQueryable<ExpenseCategoryDto> GetExpenseCategories([Service] IFinanceDataContext context)
    {
        return context.ExpenseCategories
            .AsNoTracking()
            .Select(c => new ExpenseCategoryDto
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description,
                IsActive = c.IsActive
            });
    }

    /// <summary>
    /// Gets a paged, filterable, and sortable list of recorded expenses.
    /// </summary>
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<ExpenseDto> GetExpenses([Service] IFinanceDataContext context)
    {
        return context.Expenses
            .Include(e => e.Category)
            .AsNoTracking()
            .Select(e => new ExpenseDto
            {
                ExpenseId = e.ExpenseId,
                CategoryId = e.CategoryId,
                CategoryName = e.Category != null ? e.Category.Name : string.Empty,
                Amount = e.Amount,
                Description = e.Description,
                Source = e.Source.ToString(),
                ShiftId = e.ShiftId,
                CreatedAt = e.CreatedAt,
                CreatedBy = e.CreatedBy
            });
    }

    /// <summary>
    /// Gets a paged, filterable, and sortable list of financial obligations (payables & receivables).
    /// </summary>
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<FinancialObligationDto> GetFinancialObligations([Service] IFinanceDataContext context)
    {
        return context.FinancialObligations
            .Include(o => o.Settlements)
            .AsNoTracking()
            .Select(o => new FinancialObligationDto
            {
                ObligationId = o.ObligationId,
                Title = o.Title,
                Type = o.Type.ToString(),
                Category = o.Category.ToString(),
                TotalAmount = o.TotalAmount,
                PaidAmount = o.PaidAmount,
                RemainingAmount = o.RemainingAmount,
                DueDate = o.DueDate,
                Status = o.Status.ToString(),
                Notes = o.Notes,
                CreatedAt = o.CreatedAt,
                Settlements = o.Settlements.Select(s => new ObligationSettlementDto
                {
                    SettlementId = s.SettlementId,
                    ObligationId = s.ObligationId,
                    AmountPaid = s.AmountPaid,
                    Source = s.Source.ToString(),
                    ShiftId = s.ShiftId,
                    Notes = s.Notes,
                    SettledAt = s.SettledAt,
                    SettledBy = s.SettledBy
                }).ToList()
            });
    }

    /// <summary>
    /// Gets a single financial obligation by ID with all settlements.
    /// </summary>
    [UseFirstOrDefault]
    public IQueryable<FinancialObligationDto> GetFinancialObligationById(
        Guid obligationId,
        [Service] IFinanceDataContext context)
    {
        return context.FinancialObligations
            .Include(o => o.Settlements)
            .AsNoTracking()
            .Where(o => o.ObligationId == obligationId)
            .Select(o => new FinancialObligationDto
            {
                ObligationId = o.ObligationId,
                Title = o.Title,
                Type = o.Type.ToString(),
                Category = o.Category.ToString(),
                TotalAmount = o.TotalAmount,
                PaidAmount = o.PaidAmount,
                RemainingAmount = o.RemainingAmount,
                DueDate = o.DueDate,
                Status = o.Status.ToString(),
                Notes = o.Notes,
                CreatedAt = o.CreatedAt,
                Settlements = o.Settlements.Select(s => new ObligationSettlementDto
                {
                    SettlementId = s.SettlementId,
                    ObligationId = s.ObligationId,
                    AmountPaid = s.AmountPaid,
                    Source = s.Source.ToString(),
                    ShiftId = s.ShiftId,
                    Notes = s.Notes,
                    SettledAt = s.SettledAt,
                    SettledBy = s.SettledBy
                }).ToList()
            });
    }

    /// <summary>
    /// Gets obligation settlements.
    /// </summary>
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<ObligationSettlementDto> GetObligationSettlements(
        Guid? obligationId,
        [Service] IFinanceDataContext context)
    {
        var query = context.ObligationSettlements.AsNoTracking();

        if (obligationId.HasValue)
        {
            query = query.Where(s => s.ObligationId == obligationId.Value);
        }

        return query.Select(s => new ObligationSettlementDto
        {
            SettlementId = s.SettlementId,
            ObligationId = s.ObligationId,
            AmountPaid = s.AmountPaid,
            Source = s.Source.ToString(),
            ShiftId = s.ShiftId,
            Notes = s.Notes,
            SettledAt = s.SettledAt,
            SettledBy = s.SettledBy
        });
    }

    /// <summary>
    /// Gets aggregated financial overview metrics.
    /// </summary>
    public async Task<FinanceDashboardSummaryDto> GetFinanceDashboardSummary(
        [Service] IFinanceDataContext context,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        var todayStart = new DateTime(now.Year, now.Month, now.Day, 0, 0, 0, DateTimeKind.Utc);
        var monthStart = new DateTime(now.Year, now.Month, 1, 0, 0, 0, DateTimeKind.Utc);

        var drawerInflows = await context.CashMovements
            .Where(m => m.Source == PaymentSource.Drawer &&
                        (m.Type == CashMovementType.OpeningFloat ||
                         m.Type == CashMovementType.Deposit ||
                         m.Type == CashMovementType.SaleCash ||
                         m.Type == CashMovementType.CustomerPayment))
            .SumAsync(m => (decimal?)m.Amount, cancellationToken) ?? 0m;

        var drawerOutflows = await context.CashMovements
            .Where(m => m.Source == PaymentSource.Drawer &&
                        (m.Type == CashMovementType.Withdrawal ||
                         m.Type == CashMovementType.ExpensePayment ||
                         m.Type == CashMovementType.ObligationSettlement))
            .SumAsync(m => (decimal?)m.Amount, cancellationToken) ?? 0m;

        var safeInflows = await context.CashMovements
            .Where(m => m.Source == PaymentSource.MainSafe &&
                        (m.Type == CashMovementType.Deposit ||
                         m.Type == CashMovementType.SaleCash ||
                         m.Type == CashMovementType.CustomerPayment))
            .SumAsync(m => (decimal?)m.Amount, cancellationToken) ?? 0m;

        var safeOutflows = await context.CashMovements
            .Where(m => m.Source == PaymentSource.MainSafe &&
                        (m.Type == CashMovementType.Withdrawal ||
                         m.Type == CashMovementType.ExpensePayment ||
                         m.Type == CashMovementType.ObligationSettlement))
            .SumAsync(m => (decimal?)m.Amount, cancellationToken) ?? 0m;

        var bankInflows = await context.CashMovements
            .Where(m => m.Source == PaymentSource.Bank &&
                        (m.Type == CashMovementType.Deposit ||
                         m.Type == CashMovementType.SaleCash ||
                         m.Type == CashMovementType.CustomerPayment))
            .SumAsync(m => (decimal?)m.Amount, cancellationToken) ?? 0m;

        var bankOutflows = await context.CashMovements
            .Where(m => m.Source == PaymentSource.Bank &&
                        (m.Type == CashMovementType.Withdrawal ||
                         m.Type == CashMovementType.ExpensePayment ||
                         m.Type == CashMovementType.ObligationSettlement))
            .SumAsync(m => (decimal?)m.Amount, cancellationToken) ?? 0m;

        var payables = await context.FinancialObligations
            .Where(o => o.Type == ObligationType.Payable && o.Status != ObligationStatus.Cancelled)
            .SumAsync(o => (decimal?)o.RemainingAmount, cancellationToken) ?? 0m;

        var receivables = await context.FinancialObligations
            .Where(o => o.Type == ObligationType.Receivable && o.Status != ObligationStatus.Cancelled)
            .SumAsync(o => (decimal?)o.RemainingAmount, cancellationToken) ?? 0m;

        var expensesToday = await context.Expenses
            .Where(e => e.CreatedAt >= todayStart)
            .SumAsync(e => (decimal?)e.Amount, cancellationToken) ?? 0m;

        var expensesMonth = await context.Expenses
            .Where(e => e.CreatedAt >= monthStart)
            .SumAsync(e => (decimal?)e.Amount, cancellationToken) ?? 0m;

        var openShifts = await context.Shifts
            .CountAsync(s => s.Status == ShiftStatus.Open, cancellationToken);

        return new FinanceDashboardSummaryDto
        {
            TotalCashInDrawers = drawerInflows - drawerOutflows,
            TotalSafeCash = safeInflows - safeOutflows,
            TotalBankBalance = bankInflows - bankOutflows,
            TotalPayablesPending = payables,
            TotalReceivablesPending = receivables,
            TotalExpensesToday = expensesToday,
            TotalExpensesThisMonth = expensesMonth,
            ActiveOpenShiftsCount = openShifts
        };
    }
}
