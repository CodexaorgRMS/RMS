using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Purchases.Application.Abstractions;
using SharedContracts.Inventory.Interfaces;

namespace Purchases.Application.Features.Purchases.Commands.Update;

public sealed class UpdatePurchaseCommandValidator
    : AbstractValidator<UpdatePurchaseCommand>
{
    private readonly IPurchasesDataContext _context;
    private readonly IProductService _productService;

    public UpdatePurchaseCommandValidator(
        IPurchasesDataContext context,
        IProductService productService)
    {
        _context = context;
        _productService = productService;

        RuleFor(x => x.PurchaseId)
            .NotEmpty();

        RuleFor(x => x.SupplierId)
            .NotEmpty()
            .WithMessage("SupplierId is required.")
            .MustAsync(SupplierExistsAndActive)
            .WithMessage("Supplier not found or inactive.");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Purchase must contain at least one item.");

        RuleForEach(x => x.Items)
            .ChildRules(item =>
            {
                item.RuleFor(i => i.ProductId)
                    .NotEmpty();

                item.RuleFor(i => i.Quantity)
                    .GreaterThan(0);

                item.RuleFor(i => i.UnitCost)
                    .GreaterThanOrEqualTo(0);
            });

        RuleFor(x => x.Items)
            .Must(items =>
                items.Select(i => i.ProductId)
                     .Distinct()
                     .Count() == items.Count)
            .WithMessage("Duplicate products are not allowed.");

        RuleFor(x => x)
            .MustAsync(async (command, cancellation) =>
            {
                foreach (var item in command.Items)
                {
                    var exists = await _productService
                        .ProductExistsAsync(item.ProductId);

                    if (!exists)
                        return false;
                }

                return true;
            })
            .WithMessage("One or more products do not exist.");
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