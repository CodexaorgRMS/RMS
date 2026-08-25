namespace Finance.Presentation.Dtos;

public sealed record FinanceDashboardSummaryDto
{
    public decimal TotalCashInDrawers { get; init; }
    public decimal TotalSafeCash { get; init; }
    public decimal TotalBankBalance { get; init; }
    public decimal TotalPayablesPending { get; init; }
    public decimal TotalReceivablesPending { get; init; }
    public decimal TotalExpensesToday { get; init; }
    public decimal TotalExpensesThisMonth { get; init; }
    public int ActiveOpenShiftsCount { get; init; }
}
