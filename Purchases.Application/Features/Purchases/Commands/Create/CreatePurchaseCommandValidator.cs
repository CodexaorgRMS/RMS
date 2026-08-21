using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Purchases.Application.Abstractions;
using SharedContracts.Inventory.Dtos;
using SharedContracts.Inventory.Interfaces;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Purchases.Application.Features.Purchases.Commands.Create
{
    public class CreatePurchaseCommandValidator : AbstractValidator<CreatePurchaseCommand>
    {
        private readonly IPurchasesDataContext _context;
        private readonly IProductService _productService;

        public CreatePurchaseCommandValidator(
            IPurchasesDataContext context,
            IProductService productService)
        {
            _context = context;
            _productService = productService;

            RuleFor(x => x.SupplierId)
                .NotEmpty()
                .WithMessage("SupplierId is required.")
                .MustAsync(SupplierExistsAndActive)
                .WithMessage("Supplier not found or inactive.");

            RuleFor(x => x.Items)
                .NotEmpty()
                .WithMessage("Purchase must contain at least one item.");

            RuleFor(x => x.Items)
                .Must(HaveUniqueProducts)
                .WithMessage("Duplicate ProductId is not allowed within the same purchase.")
                .When(x => x.Items is { Count: > 0 });

            RuleForEach(x => x.Items)
                .ChildRules(item =>
                {
                    item.RuleFor(i => i.ProductId)
                        .NotEmpty()
                        .WithMessage("ProductId is required.")
                        .MustAsync(async (productId, cancellationToken) =>
                            await _productService.ProductExistsAsync(productId))
                        .WithMessage("Product not found.");

                    item.RuleFor(i => i.Quantity)
                        .GreaterThan(0)
                        .WithMessage("Quantity must be greater than zero.");

                    item.RuleFor(i => i.UnitCost)
                        .GreaterThanOrEqualTo(0)
                        .WithMessage("UnitCost must be zero or greater.");
                });

            RuleFor(x => x.IdempotencyKey)
                .NotEmpty()
                .MaximumLength(200);
        }

        private static bool HaveUniqueProducts(
            IReadOnlyCollection<CreatePurchaseItemDto> items)
        {
            return items.Select(i => i.ProductId).Distinct().Count() == items.Count;
        }

        private async Task<bool> SupplierExistsAndActive(
            Guid supplierId,
            CancellationToken cancellationToken)
        {
            var supplier = await _context.Suppliers
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.SupplierId == supplierId, cancellationToken);

            return supplier is { IsActive: true };
        }
    }
}