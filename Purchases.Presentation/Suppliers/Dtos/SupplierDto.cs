using System;

namespace Purchases.Presentation.Suppliers.Dtos;

public record SupplierDto(
    Guid SupplierId,
    string Name,
    string? Phone,
    string? Email,
    string? Address,
    bool IsActive,
    DateTime CreatedAt);
