using FluentResults;
using Finance.Application.Abstractions;
using Finance.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Wolverine.Attributes;

namespace Finance.Application.Features.Expenses.Commands.CreateExpenseCategory;

[Transactional]
public static class CreateExpenseCategoryCommandHandler
{
    public static async Task<Result<Guid>> Handle(
        CreateExpenseCategoryCommand command,
        IFinanceDataContext context,
        CancellationToken cancellationToken)
    {
        var exists = await context.ExpenseCategories
            .AnyAsync(c => c.Name.ToLower() == command.Name.Trim().ToLower(), cancellationToken);

        if (exists)
        {
            return Result.Fail<Guid>("An expense category with the same name already exists.");
        }

        var category = new ExpenseCategory
        {
            Name = command.Name.Trim(),
            Description = command.Description,
            IsActive = true
        };

        context.ExpenseCategories.Add(category);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok(category.CategoryId);
    }
}
