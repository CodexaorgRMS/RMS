using FluentValidation;
using Inventory.Application.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Features.InventoryItems.Commands.Create;

public sealed class CreateInventoryItemCommandValidator
    : AbstractValidator<CreateInventoryItemCommand>
{
    private readonly IInventoryDataContext _context;

    public CreateInventoryItemCommandValidator(
        IInventoryDataContext context)
    {
        _context = context;

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .WithMessage("Product id is required.")
            .MustAsync(ProductExists)
            .WithMessage("Product does not exist.")
            .MustAsync(ProductDoesNotHaveInventoryItem)
            .WithMessage("Inventory item already exists for this product.");

        RuleFor(x => x.Quantity)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Quantity cannot be negative.");

        RuleFor(x => x.MinStock)
            .GreaterThanOrEqualTo(0)
            .WithMessage("Minimum stock cannot be negative.");
    }

    private async Task<bool> ProductExists(
        Guid productId,
        CancellationToken cancellationToken)
    {
        return await _context.Products
            .AnyAsync(
                x => x.ProductId == productId,
                cancellationToken);
    }

    private async Task<bool> ProductDoesNotHaveInventoryItem(
        Guid productId,
        CancellationToken cancellationToken)
    {
        return !await _context.InventoryItems
            .AnyAsync(
                x => x.ProductId == productId,
                cancellationToken);
    }
}