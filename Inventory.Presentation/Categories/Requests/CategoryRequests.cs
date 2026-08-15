using Inventory.Domain.Enums;
using System;

namespace Inventory.Presentation.Categories.Requests
{
    public record CreateCategoryRequest(string Name, string Description, Guid? ParentId);

    public record UpdateCategoryRequest(Guid CategoryId, string Name, string Description, Guid? ParentId);

    public record UpdateCategoryPickingStrategyRequest(Guid CategoryId, PickingStrategy PickingStrategy);
    public sealed record UpdateCategoryExpiryRuleRequest(int ExpiryWarningDays, decimal AutoMarkdownPercentage);
}
