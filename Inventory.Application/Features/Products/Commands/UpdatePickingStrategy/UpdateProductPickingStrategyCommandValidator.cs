using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Inventory.Application.Features.Products.Commands.UpdatePickingStrategy
{
	public class UpdateProductPickingStrategyCommandValidator: AbstractValidator<UpdateProductPickingStrategyCommand>
	{
		public UpdateProductPickingStrategyCommandValidator()
		{
			RuleFor(x => x.ProductId).NotEmpty().WithMessage("ProductId is required.");
			RuleFor(x => x.PickingStrategy).IsInEnum().WithMessage("Invalid PickingStrategy value.");
		}				
	}
}
