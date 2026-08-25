using FluentValidation;

namespace Offers.Presentation.Offers.Requests.Validators;

public sealed class CreateOfferTargetRequestValidator : AbstractValidator<CreateOfferTargetRequest>
{
    public CreateOfferTargetRequestValidator()
    {
        RuleFor(x => x.TargetId)
            .NotEmpty().WithMessage("Target ID is required.");

        RuleFor(x => x.TargetType)
            .IsInEnum().WithMessage("Invalid target type.");

        RuleFor(x => x.RequiredQuantity)
            .GreaterThan(0).WithMessage("Required quantity must be at least 1.");

        RuleFor(x => x.SpecialPrice)
            .GreaterThanOrEqualTo(0).WithMessage("Special price must be non-negative.")
            .When(x => x.SpecialPrice.HasValue);
    }
}

public sealed class CreatePercentageOfferRequestValidator : AbstractValidator<CreatePercentageOfferRequest>
{
    public CreatePercentageOfferRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Offer name is required.")
            .MaximumLength(255).WithMessage("Offer name cannot exceed 255 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Offer description cannot exceed 1000 characters.");

        RuleFor(x => x.Percentage)
            .InclusiveBetween(0.01m, 100m).WithMessage("Percentage discount must be between 0.01% and 100%.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.");

        RuleFor(x => x.Priority)
            .GreaterThanOrEqualTo(0).WithMessage("Priority must be non-negative.");

        RuleForEach(x => x.Targets)
            .SetValidator(new CreateOfferTargetRequestValidator())
            .When(x => x.Targets != null);
    }
}

public sealed class CreateFixedOfferRequestValidator : AbstractValidator<CreateFixedOfferRequest>
{
    public CreateFixedOfferRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Offer name is required.")
            .MaximumLength(255).WithMessage("Offer name cannot exceed 255 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Offer description cannot exceed 1000 characters.");

        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage("Fixed discount amount must be greater than zero.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.");

        RuleFor(x => x.Priority)
            .GreaterThanOrEqualTo(0).WithMessage("Priority must be non-negative.");

        RuleForEach(x => x.Targets)
            .SetValidator(new CreateOfferTargetRequestValidator())
            .When(x => x.Targets != null);
    }
}

public sealed class CreateBogoOfferRequestValidator : AbstractValidator<CreateBogoOfferRequest>
{
    public CreateBogoOfferRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Offer name is required.")
            .MaximumLength(255).WithMessage("Offer name cannot exceed 255 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Offer description cannot exceed 1000 characters.");

        RuleFor(x => x.DiscountPercentage)
            .InclusiveBetween(0m, 100m).WithMessage("BOGO discount percentage must be between 0% and 100%.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.");

        RuleFor(x => x.Priority)
            .GreaterThanOrEqualTo(0).WithMessage("Priority must be non-negative.");

        RuleForEach(x => x.Targets)
            .SetValidator(new CreateOfferTargetRequestValidator())
            .When(x => x.Targets != null);
    }
}

public sealed class CreateBundleOfferRequestValidator : AbstractValidator<CreateBundleOfferRequest>
{
    public CreateBundleOfferRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Offer name is required.")
            .MaximumLength(255).WithMessage("Offer name cannot exceed 255 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Offer description cannot exceed 1000 characters.");

        RuleFor(x => x.BundlePrice)
            .GreaterThanOrEqualTo(0).WithMessage("Bundle price must be non-negative.");

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.");

        RuleFor(x => x.Priority)
            .GreaterThanOrEqualTo(0).WithMessage("Priority must be non-negative.");

        RuleFor(x => x.Targets)
            .NotEmpty().WithMessage("Bundle offers must contain at least one target.");

        RuleForEach(x => x.Targets)
            .SetValidator(new CreateOfferTargetRequestValidator())
            .When(x => x.Targets != null);
    }
}

public sealed class ToggleOfferStatusRequestValidator : AbstractValidator<ToggleOfferStatusRequest>
{
    public ToggleOfferStatusRequestValidator()
    {
    }
}
