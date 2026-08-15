using FluentResults;
using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Wolverine.Attributes;

namespace Inventory.Application.Features.Categories.Commands.UpdateExpiryRule;

[Transactional]
public static class UpdateCategoryExpiryRuleHandler
{
    public static async Task<Result> Handle(
        UpdateCategoryExpiryRuleCommand command,
        IInventoryDataContext context,
        CancellationToken cancellationToken)
    {
        var category = await context.Categories
            .FirstOrDefaultAsync(c => c.CategoryId == command.CategoryId, cancellationToken);

        if (category is null)
            return Result.Fail($"Category '{command.CategoryId}' not found.");

        category.ExpiryWarningDays = command.ExpiryWarningDays;
        category.AutoMarkdownPercentage = command.AutoMarkdownPercentage;

        return Result.Ok();
    }
}