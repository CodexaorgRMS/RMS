using FluentValidation;
using Offers.Application.Abstractions;
using Offers.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace Offers.Application.Features.Offers.Commands.Create;

public sealed class CreateOfferCommandValidator : AbstractValidator<CreateOfferCommand>
{
    public CreateOfferCommandValidator() // IOffersDbContext context
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Offer name is required.")
            .MaximumLength(255).WithMessage("Offer name cannot exceed 255 characters.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Offer description cannot exceed 1000 characters.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid offer type.");

        RuleFor(x => x.Value)
            .GreaterThan(0).WithMessage("Offer value must be greater than zero.")
            .When(x => x.Type != OfferType.Bogo);

        RuleFor(x => x.Value)
            .InclusiveBetween(0.01m, 100m).WithMessage("Percentage discount must be between 0.01% and 100%.")
            .When(x => x.Type == OfferType.Percentage);

        RuleFor(x => x.StartDate)
            .NotEmpty().WithMessage("Start date is required.");

        RuleFor(x => x.EndDate)
            .NotEmpty().WithMessage("End date is required.")
            .GreaterThan(x => x.StartDate).WithMessage("End date must be after start date.");

        RuleFor(x => x.Priority)
            .GreaterThanOrEqualTo(0).WithMessage("Priority must be non-negative.");

        RuleFor(x => x.Targets)
            .NotEmpty().WithMessage("Bundle offers must contain at least one target.")
            .When(x => x.Type == OfferType.Bundle);

        RuleForEach(x => x.Targets).ChildRules(target =>
        {
            target.RuleFor(t => t.TargetId)
                .NotEmpty().WithMessage("Target ID is required.");

            target.RuleFor(t => t.TargetType)
                .IsInEnum().WithMessage("Invalid target type.");

            target.RuleFor(t => t.RequiredQuantity)
                .GreaterThan(0).WithMessage("Required quantity must be at least 1.");

            target.RuleFor(t => t.SpecialPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Special price must be non-negative.")
                .When(t => t.SpecialPrice.HasValue);
        }).When(x => x.Targets != null);
    }
}
