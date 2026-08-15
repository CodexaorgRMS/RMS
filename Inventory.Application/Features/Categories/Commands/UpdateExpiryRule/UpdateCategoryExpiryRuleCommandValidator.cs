using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application.Features.Categories.Commands.UpdateExpiryRule;

public sealed class UpdateCategoryExpiryRuleCommandValidator : AbstractValidator<UpdateCategoryExpiryRuleCommand>
{
    public UpdateCategoryExpiryRuleCommandValidator()
    {
        RuleFor(x => x.CategoryId).NotEmpty();
        RuleFor(x => x.ExpiryWarningDays).GreaterThan(0).WithMessage("Warning days must be at least 1.");
        RuleFor(x => x.AutoMarkdownPercentage).InclusiveBetween(0, 100).WithMessage("Discount percentage must be between 0 and 100.");
    }
}