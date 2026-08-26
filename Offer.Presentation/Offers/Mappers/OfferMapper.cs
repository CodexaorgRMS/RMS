using Offers.Application.Features.Offers.Commands.Create;
using Offers.Application.Features.Offers.Commands.Delete;
using Offers.Application.Features.Offers.Commands.EvaluateCart;
using Offers.Application.Features.Offers.Commands.ToggleStatus;
using Offers.Application.Features.Offers.DTOs;
using Offers.Domain.Entities;
using Offers.Domain.Enums;
using Offers.Domain.Models;
using Offers.Presentation.Offers.Dtos;
using Offers.Presentation.Offers.Requests;
using Riok.Mapperly.Abstractions;

namespace Offers.Presentation.Offers.Mappers;

[Mapper]
public partial class OfferMapper
{
    public CreateOfferCommand MapToCommand(CreatePercentageOfferRequest request)
    {
        return new CreateOfferCommand(
            Name: request.Name,
            Description: request.Description,
            Type: OfferType.Percentage,
            Value: request.Percentage,
            StartDate: request.StartDate,
            EndDate: request.EndDate,
            Priority: request.Priority,
            IsSmart: false,
            Targets: request.Targets?.Select(MapTargetToDto).ToList()
        );
    }

    public CreateOfferCommand MapToCommand(CreateFixedOfferRequest request)
    {
        return new CreateOfferCommand(
            Name: request.Name,
            Description: request.Description,
            Type: OfferType.Fixed,
            Value: request.Amount,
            StartDate: request.StartDate,
            EndDate: request.EndDate,
            Priority: request.Priority,
            IsSmart: false,
            Targets: request.Targets?.Select(MapTargetToDto).ToList()
        );
    }

    public CreateOfferCommand MapToCommand(CreateBogoOfferRequest request)
    {
        return new CreateOfferCommand(
            Name: request.Name,
            Description: request.Description,
            Type: OfferType.Bogo,
            Value: request.DiscountPercentage,
            StartDate: request.StartDate,
            EndDate: request.EndDate,
            Priority: request.Priority,
            IsSmart: false,
            Targets: request.Targets?.Select(MapTargetToDto).ToList()
        );
    }

    public CreateOfferCommand MapToCommand(CreateBundleOfferRequest request)
    {
        return new CreateOfferCommand(
            Name: request.Name,
            Description: request.Description,
            Type: OfferType.Bundle,
            Value: request.BundlePrice,
            StartDate: request.StartDate,
            EndDate: request.EndDate,
            Priority: request.Priority,
            IsSmart: false,
            Targets: request.Targets?.Select(MapTargetToDto).ToList()
        );
    }

    public ToggleOfferStatusCommand MapToCommand(Guid offerId, ToggleOfferStatusRequest request)
    {
        return new ToggleOfferStatusCommand(offerId, request.IsActive);
    }

    public EvaluateCartOffersCommand MapToCommand(EvaluateCartRequest request)
    {
        var items = request.Items.Select(i => new CartItemInput(
            i.ProductId,
            i.CategoryId,
            i.Quantity,
            i.UnitPrice
        )).ToList();

        return new EvaluateCartOffersCommand(items);
    }

    public CreateOfferTargetDto MapTargetToDto(CreateOfferTargetRequest request)
    {
        return new CreateOfferTargetDto(
            request.TargetType,
            request.TargetId,
            request.RequiredQuantity,
            request.SpecialPrice
        );
    }

    public OfferTargetDto MapToDto(OfferTarget target)
    {
        return new OfferTargetDto(
            target.OfferTargetId,
            target.OfferId,
            target.TargetType.ToString(),
            target.TargetId,
            target.RequiredQuantity,
            target.SpecialPrice
        );
    }

    public OfferDto MapToDto(Offer offer)
    {
        return new OfferDto(
            offer.OfferId,
            offer.Name,
            offer.Description,
            offer.Type.ToString(),
            offer.Value,
            offer.StartDate,
            offer.EndDate,
            offer.IsActive,
            offer.IsSmart,
            offer.Priority,
            offer.CreatedAt,
            offer.Targets.Select(MapToDto).ToList()
        );
    }

    public AppliedDiscountDetailDto MapToDto(AppliedDiscountDetail detail)
    {
        return new AppliedDiscountDetailDto(
            detail.OfferId,
            detail.OfferName,
            detail.DiscountAmount,
            detail.Description
        );
    }

    public CartEvaluationDto MapToDto(DiscountEvaluationResult result)
    {
        return new CartEvaluationDto(
            result.TotalDiscount,
            result.Details.Select(MapToDto).ToList()
        );
    }
}
