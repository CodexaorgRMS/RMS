using FluentValidation;

namespace Offers.Application.Features.Offers.Commands.ToggleStatus;

public sealed class ToggleOfferStatusCommandValidator : AbstractValidator<ToggleOfferStatusCommand>
{
    public ToggleOfferStatusCommandValidator()
    {
        RuleFor(x => x.OfferId)
            .NotEmpty().WithMessage("Offer ID is required.");
    }
}
