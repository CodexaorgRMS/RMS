using FluentValidation;

namespace Offers.Application.Features.Offers.Commands.Delete;

public sealed class DeleteOfferCommandValidator : AbstractValidator<DeleteOfferCommand>
{
    public DeleteOfferCommandValidator()
    {
        RuleFor(x => x.OfferId)
            .NotEmpty().WithMessage("Offer ID is required.");
    }
}
