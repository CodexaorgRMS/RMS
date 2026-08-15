using FluentValidation;
using Inventory.Domain.Entities;

namespace Inventory.Presentation.Adjustments.Requests
{
	public class CreateAdjustmentRequestValidator
		: AbstractValidator<CreateAdjustmentRequest>
	{
		public CreateAdjustmentRequestValidator()
		{
			RuleFor(x => x.ProductId)
				.NotEmpty()
				.WithMessage("ProductId is required.");

			RuleFor(x => x.ProductBatchId)
				.NotEmpty()
				.WithMessage("ProductBatchId is required.");

			RuleFor(x => x.Quantity)
				.GreaterThan(0)
				.WithMessage("Quantity must be greater than 0.");

			RuleFor(x => x.Type)
				.IsInEnum()
				.WithMessage("Invalid AdjustmentType.");

			RuleFor(x => x.Reason)
				.IsInEnum()
				.WithMessage("Invalid AdjustmentReason.");

			RuleFor(x => x)
				.Must(HasValidTypeAndReason)
				.WithMessage(
					"Invalid combination of AdjustmentType and AdjustmentReason.");
		}

		private static bool HasValidTypeAndReason(
			CreateAdjustmentRequest request)
		{
			return request.Type switch
			{
				AdjustmentType.Increase =>
					request.Reason is
						AdjustmentReason.Miscount or
						AdjustmentReason.Other,

				AdjustmentType.Decrease =>
					request.Reason is
						AdjustmentReason.Damaged or
						AdjustmentReason.Expired or
						AdjustmentReason.Theft or
						AdjustmentReason.Miscount or
						AdjustmentReason.Other,

				_ => false
			};
		}
	}
}
