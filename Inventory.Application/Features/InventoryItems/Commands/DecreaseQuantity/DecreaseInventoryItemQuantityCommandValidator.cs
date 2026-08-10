using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application.Features.InventoryItems.Commands.DecreaseQuantity
{
    public sealed class DecreaseInventoryItemQuantityCommandValidator
    : AbstractValidator<DecreaseInventoryItemQuantityCommand>
    {
        public DecreaseInventoryItemQuantityCommandValidator()
        {
            RuleFor(x => x.InventoryItemId)
                .NotEmpty()
                .WithMessage("Inventory item id is required.");

            RuleFor(x => x.Quantity)
                .GreaterThan(0)
                .WithMessage("Quantity must be greater than zero.");
        }
    }
}
