using Customers.Domain.Enums;

namespace Customers.Presentation.Dtos;

public record CustomerLedgerDto(
	Guid LedgerId,
	Guid CustomerId,
	LedgerType Type,
	decimal Amount,
	Guid? ReferenceOrderId,
	DateTime CreatedAt
);
