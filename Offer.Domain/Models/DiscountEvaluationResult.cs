namespace Offers.Domain.Models;

public sealed record DiscountEvaluationResult(
    decimal TotalDiscount,
    List<AppliedDiscountDetail> Details)
{
    public static DiscountEvaluationResult Empty => new(0m, []);
}
