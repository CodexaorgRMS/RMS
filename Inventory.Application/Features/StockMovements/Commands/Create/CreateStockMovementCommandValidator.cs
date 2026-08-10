using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application.Features.StockMovements.Commands.Create;

public sealed class CreateStockMovementCommandValidator : AbstractValidator<CreateStockMovementCommand>
{
    public CreateStockMovementCommandValidator()
    {
        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.");

        RuleFor(x => x.Type)
            .Must(type => type is "IN" or "OUT" or "ADJUST")
            .WithMessage("Type must be IN, OUT, or ADJUST.");

        RuleFor(x => x.Quantity)
            .NotEmpty().WithMessage("Quantity is required and cannot be zero.");

        RuleFor(x => x.ReferenceId)
            .NotEmpty().WithMessage("ReferenceId is required.");
    }
}