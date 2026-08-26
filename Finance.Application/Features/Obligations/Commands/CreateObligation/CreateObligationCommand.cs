using Finance.Domain.Enums;

namespace Finance.Application.Features.Obligations.Commands.CreateObligation;

public sealed record CreateObligationCommand(
    string Title,
    ObligationType Type,
    ObligationCategory Category,
    decimal TotalAmount,
    DateTime? DueDate,
    string? Notes);
