using FluentValidation;

namespace Inventory.Presentation.Categories.Requests
{
    public class CreateCategoryRequestValidator : AbstractValidator<CreateCategoryRequest>
    {
        public CreateCategoryRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Description).MaximumLength(1000);
        }
    }

    public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequest>
    {
        public UpdateCategoryRequestValidator()
        {
            RuleFor(x => x.CategoryId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Description).MaximumLength(1000);
        }
    }

    public class UpdateCategoryPickingStrategyRequestValidator : AbstractValidator<UpdateCategoryPickingStrategyRequest>
    {
        public UpdateCategoryPickingStrategyRequestValidator()
        {
            RuleFor(x => x.CategoryId).NotEmpty();
            RuleFor(x => x.PickingStrategy).IsInEnum();
        }
	}

    public sealed class UpdateCategoryExpiryRuleRequestValidator : AbstractValidator<UpdateCategoryExpiryRuleRequest>
    {
        public UpdateCategoryExpiryRuleRequestValidator()
        {
            RuleFor(x => x.ExpiryWarningDays)
                .GreaterThan(0)
                .WithMessage("Expiry warning days must be greater than 0.");

            RuleFor(x => x.AutoMarkdownPercentage)
                .InclusiveBetween(0, 100)
                .WithMessage("Auto markdown percentage must be between 0% and 100%.");
        }
    }
}
