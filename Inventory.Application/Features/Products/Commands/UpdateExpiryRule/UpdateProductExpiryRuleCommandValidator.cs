using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application.Features.Products.Commands.UpdateExpiryRule;

public sealed class UpdateProductExpiryRuleCommandValidator : AbstractValidator<UpdateProductExpiryRuleCommand>
{
    public UpdateProductExpiryRuleCommandValidator()
    {
        RuleFor(x => x.ProductId).NotEmpty();
        RuleFor(x => x.CustomExpiryWarningDays)
            .GreaterThan(0)
            .When(x => x.CustomExpiryWarningDays.HasValue)
            .WithMessage("Warning days must be greater than 0.");

        RuleFor(x => x.CustomAutoMarkdownPercentage)
            .InclusiveBetween(0, 100)
            .When(x => x.CustomAutoMarkdownPercentage.HasValue)
            .WithMessage("Discount percentage must be between 0 and 100.");
    }
}