using FluentValidation;
using Inventory.Application.Abstractions;
using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Application.Features.StockMovements.Commands.Create;

public sealed class CreateStockMovementCommandValidator : AbstractValidator<CreateStockMovementCommand>
{
    private readonly IInventoryDataContext _context;

    public CreateStockMovementCommandValidator(IInventoryDataContext context)
    {
        _context = context;

        RuleFor(x => x.ProductId)
            .NotEmpty().WithMessage("ProductId is required.")
            .MustAsync(ProductExists).WithMessage("Product does not exist.")
            .MustAsync(InventoryItemExists).WithMessage("Inventory item not found for this product.");

        RuleFor(x => x.Type)
            .IsInEnum().WithMessage("Invalid StockMovementType.");

        RuleFor(x => x.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");

        RuleFor(x => x.ReferenceId)
            .NotEmpty().WithMessage("ReferenceId is required.");

        RuleFor(x => x)
            .MustAsync(HasSufficientStockForDecrease)
            .When(x => x.Type is StockMovementType.Out or StockMovementType.Adjustment)
            .WithMessage("Insufficient stock for this movement.");
    }

    private async Task<bool> ProductExists(Guid productId, CancellationToken cancellationToken)
    {
        return await _context.Products.AnyAsync(p => p.ProductId == productId, cancellationToken);
    }

    private async Task<bool> InventoryItemExists(Guid productId, CancellationToken cancellationToken)
    {
        return await _context.InventoryItems.AnyAsync(i => i.ProductId == productId, cancellationToken);
    }

    private async Task<bool> HasSufficientStockForDecrease(CreateStockMovementCommand command, CancellationToken cancellationToken)
    {
        return await _context.InventoryItems
            .AnyAsync(i => i.ProductId == command.ProductId && i.Quantity >= command.Quantity, cancellationToken);
    }
}