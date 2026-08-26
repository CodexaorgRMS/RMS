namespace Offers.Presentation.Offers.Dtos;

public sealed record CartEvaluationDto(
    decimal TotalDiscount,
    List<AppliedDiscountDetailDto> Details);
