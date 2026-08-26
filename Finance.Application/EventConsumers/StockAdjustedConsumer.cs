using Finance.Application.Abstractions;
using Finance.Domain.Entities;
using Finance.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Inventory.Events;
using Wolverine.Attributes;

namespace Finance.Application.EventConsumers;

[Transactional]
public static class StockAdjustedConsumer
{
    public static async Task Handle(
        StockAdjustedIntegrationEvent @event,
        IFinanceDataContext context,
        CancellationToken cancellationToken)
    {
        // If stock adjustment is negative / write-off / loss with financial impact
        if (@event.Quantity < 0 && @event.UnitCost > 0)
        {
            var lossAmount = Math.Abs(@event.Quantity) * @event.UnitCost;

            // Find or ensure an Inventory Loss expense category exists
            var category = await context.ExpenseCategories
                .FirstOrDefaultAsync(c => c.Name == "Inventory Loss / Shrinkage", cancellationToken);

            if (category is null)
            {
                category = new ExpenseCategory
                {
                    Name = "Inventory Loss / Shrinkage",
                    Description = "Auto-generated category for inventory write-offs and damaged stock",
                    IsActive = true
                };
                context.ExpenseCategories.Add(category);
                await context.SaveChangesAsync(cancellationToken);
            }

            var expense = new Expense
            {
                CategoryId = category.CategoryId,
                Amount = lossAmount,
                Description = $"Stock Adjustment ({(@event.AdjustmentType ?? "Loss")}): Product {@event.ProductId} (Qty: {@event.Quantity}, Cost: {@event.UnitCost:C})",
                Source = PaymentSource.MainSafe,
                ShiftId = null,
                CreatedBy = Guid.Empty,
                CreatedAt = @event.OccurredAt
            };

            context.Expenses.Add(expense);
            await context.SaveChangesAsync(cancellationToken);
        }
    }
}
