using FluentValidation;
using Inventory.Domain.Enums;
using System;

namespace Inventory.Presentation.Products.Requests
{
    public record CreateProductRequest(string Name, string Description, Guid CategoryId);
    public record UpdateProductRequest(Guid ProductId, string Name, string Description, Guid CategoryId);
    public record UpdateProductPickingStrategyRequest(Guid ProductId, PickingStrategy PickingStrategy);
    public sealed record UpdateProductExpiryRuleRequest(int? CustomExpiryWarningDays, decimal? CustomAutoMarkdownPercentage);
}
