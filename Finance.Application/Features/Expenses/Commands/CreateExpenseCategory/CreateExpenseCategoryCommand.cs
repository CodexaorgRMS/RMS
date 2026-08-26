namespace Finance.Application.Features.Expenses.Commands.CreateExpenseCategory;

public sealed record CreateExpenseCategoryCommand(
    string Name,
    string? Description);
