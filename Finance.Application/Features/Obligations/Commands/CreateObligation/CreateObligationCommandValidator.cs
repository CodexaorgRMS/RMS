using FluentValidation;

namespace Finance.Application.Features.Obligations.Commands.CreateObligation;

public sealed class CreateObligationCommandValidator : AbstractValidator<CreateObligationCommand>
{
    public CreateObligationCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Obligation title is required.")
            .MaximumLength(200)
            .WithMessage("Obligation title must not exceed 200 characters.");

        RuleFor(x => x.Type)
            .IsInEnum()
            .WithMessage("Valid obligation type is required.");

        RuleFor(x => x.Category)
            .IsInEnum()
            .WithMessage("Valid obligation category is required.");

        RuleFor(x => x.TotalAmount)
            .GreaterThan(0)
            .WithMessage("Total amount must be greater than 0.");

        RuleFor(x => x.Notes)
            .MaximumLength(500)
            .WithMessage("Notes must not exceed 500 characters.")
            .When(x => x.Notes != null);
    }
}
