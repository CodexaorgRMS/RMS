using Offers.Application.Features.Offers.DTOs;
using Offers.Domain.Enums;

namespace Offers.Application.Features.Offers.Commands.Create;

public sealed record CreateOfferCommand(
    string Name,
    string? Description,
    OfferType Type,
    decimal Value,
    DateTime StartDate,
    DateTime EndDate,
    int Priority = 1,
    bool IsSmart = false,
    List<CreateOfferTargetDto>? Targets = null);
