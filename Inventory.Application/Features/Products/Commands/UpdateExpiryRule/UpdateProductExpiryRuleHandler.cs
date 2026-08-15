using FluentResults;
using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Wolverine.Attributes;

namespace Inventory.Application.Features.Products.Commands.UpdateExpiryRule;

[Transactional]
public static class UpdateProductExpiryRuleHandler
{
    public static async Task<Result> Handle(
        UpdateProductExpiryRuleCommand command,
        IInventoryDataContext context,
        CancellationToken cancellationToken)
    {
        var product = await context.Products
            .FirstOrDefaultAsync(p => p.ProductId == command.ProductId, cancellationToken);

        if (product is null)
            return Result.Fail($"Product '{command.ProductId}' not found.");

        product.CustomExpiryWarningDays = command.CustomExpiryWarningDays;
        product.CustomAutoMarkdownPercentage = command.CustomAutoMarkdownPercentage;

        return Result.Ok();
    }
}