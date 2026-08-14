using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application.Features.Categories.Commands.UpdatePickingStrategy
{
	public class UpdateCategoryPickingStrategyCommandValidator : AbstractValidator<UpdateCategoryPickingStrategyCommand>
	{
		public UpdateCategoryPickingStrategyCommandValidator()
		{
			RuleFor(x => x.CategoryId).NotEmpty().WithMessage("CategoryId is required.");
			RuleFor(x => x.PickingStrategy).
				IsInEnum().WithMessage("Invalid PickingStrategy value.");

		}
	}
}
