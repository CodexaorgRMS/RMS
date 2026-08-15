using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application.Features.Categories.Commands.UpdateExpiryRule;

public sealed record UpdateCategoryExpiryRuleCommand(
    Guid CategoryId,
    int ExpiryWarningDays,
    decimal AutoMarkdownPercentage);