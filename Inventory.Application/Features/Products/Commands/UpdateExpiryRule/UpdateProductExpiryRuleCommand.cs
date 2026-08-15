using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application.Features.Products.Commands.UpdateExpiryRule;

public sealed record UpdateProductExpiryRuleCommand(
    Guid ProductId,
    int? CustomExpiryWarningDays,
    decimal? CustomAutoMarkdownPercentage);