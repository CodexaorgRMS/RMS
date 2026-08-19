using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Purchases.Application.Features.Suppliers.Commands.CreateSupplier
{
    public class CreateSupplierValidator : AbstractValidator<CreateSupplierCommand>
    {
        public CreateSupplierValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

            RuleFor(x => x.Phone)
                .MaximumLength(50);

            RuleFor(x => x.Email)
                .MaximumLength(200)
                .EmailAddress()
                .When(x => !string.IsNullOrWhiteSpace(x.Email));

            RuleFor(x => x.Address)
                .MaximumLength(500);

            RuleFor(x => x.IdempotencyKey)
                .NotEmpty()
                .MaximumLength(200);
        }
    }
}
