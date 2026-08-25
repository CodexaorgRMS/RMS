using FluentValidation;

namespace Finance.Application.Features.Obligations.Commands.CancelObligation;

public sealed class CancelObligationCommandValidator : AbstractValidator<CancelObligationCommand>
{
    public CancelObligationCommandValidator()
    {
        RuleFor(x => x.ObligationId)
            .NotEmpty()
            .WithMessage("Obligation ID is required.");
    }
}
