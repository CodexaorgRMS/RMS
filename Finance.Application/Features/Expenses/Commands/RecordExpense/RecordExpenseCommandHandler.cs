using FluentResults;
using Finance.Application.Abstractions;
using Finance.Domain.Entities;
using Finance.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Finance.Events;
using Wolverine;
using Wolverine.Attributes;

namespace Finance.Application.Features.Expenses.Commands.RecordExpense;

[Transactional]
public static class RecordExpenseCommandHandler
{
    public static async Task<Result<Guid>> Handle(
        RecordExpenseCommand command,
        IFinanceDataContext context,
        IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var category = await context.ExpenseCategories
            .FirstOrDefaultAsync(c => c.CategoryId == command.CategoryId, cancellationToken);

        if (category is null)
        {
            return Result.Fail<Guid>("Expense category not found.");
        }

        if (!category.IsActive)
        {
            return Result.Fail<Guid>("Expense category is inactive.");
        }

        if (command.ShiftId.HasValue)
        {
            var shift = await context.Shifts
                .FirstOrDefaultAsync(s => s.ShiftId == command.ShiftId.Value, cancellationToken);

            if (shift is null)
            {
                return Result.Fail<Guid>("Associated shift was not found.");
            }

            if (shift.Status == ShiftStatus.Closed)
            {
                return Result.Fail<Guid>("Cannot record an expense against a closed shift.");
            }
        }

        var expense = new Expense
        {
            CategoryId = command.CategoryId,
            Amount = command.Amount,
            Description = command.Description,
            Source = command.Source,
            ShiftId = command.ShiftId,
            CreatedBy = command.CreatedBy,
            CreatedAt = DateTime.UtcNow
        };

        context.Expenses.Add(expense);

        var cashMovement = new CashMovement
        {
            ShiftId = command.ShiftId,
            Amount = command.Amount,
            Type = CashMovementType.ExpensePayment,
            Description = $"Expense: {command.Description}",
            Source = command.Source,
            ReferenceId = expense.ExpenseId,
            CreatedBy = command.CreatedBy,
            CreatedAt = expense.CreatedAt
        };

        context.CashMovements.Add(cashMovement);

        await bus.PublishAsync(new ExpenseRecordedIntegrationEvent(
            expense.ExpenseId,
            expense.Amount,
            category.Name,
            expense.Source.ToString(),
            DateTime.UtcNow));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok(expense.ExpenseId);
    }
}
