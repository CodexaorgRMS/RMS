using FluentValidation;

namespace Inventory.Presentation.Products.Requests
{
    public class CreateProductRequestValidator : AbstractValidator<CreateProductRequest>
    {
        public CreateProductRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
            RuleFor(x => x.CategoryId).NotEmpty();
        }
    }

    public class UpdateProductRequestValidator : AbstractValidator<UpdateProductRequest>
    {
        public UpdateProductRequestValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
            RuleFor(x => x.Description).NotEmpty().MaximumLength(1000);
            RuleFor(x => x.CategoryId).NotEmpty();
        }
    }
    public sealed class UpdateProductExpiryRuleRequestValidator : AbstractValidator<UpdateProductExpiryRuleRequest>
    {
        public UpdateProductExpiryRuleRequestValidator()
        {
            RuleFor(x => x.CustomExpiryWarningDays)
                .GreaterThan(0)
                .When(x => x.CustomExpiryWarningDays.HasValue)
                .WithMessage("Custom expiry warning days must be greater than 0.");

            RuleFor(x => x.CustomAutoMarkdownPercentage)
                .InclusiveBetween(0, 100)
                .When(x => x.CustomAutoMarkdownPercentage.HasValue)
                .WithMessage("Custom auto markdown percentage must be between 0% and 100%.");
        }
    }
}
