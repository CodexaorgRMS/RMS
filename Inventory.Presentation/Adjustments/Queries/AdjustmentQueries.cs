using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Inventory.Application.Abstractions;
using Inventory.Presentation.Adjustments.Dtos;

using Microsoft.EntityFrameworkCore;

namespace Inventory.Presentation.Adjustments.Queries
{
	[ExtendObjectType(typeof(SharedPresentation.GraphQL.Query))]
	public class AdjustmentQueries
	{
		/// <summary>
		/// Gets a paged, filterable, and sortable list of all inventory adjustments.
		/// </summary>
		[UsePaging(IncludeTotalCount = true)]
		[UseFiltering]
		[UseSorting]
		public IQueryable<AdjustmentDto> GetAdjustments([Service] IInventoryDataContext context)
		{
			return context.Adjustments
				.AsNoTracking()
				.Select(a => new AdjustmentDto
				{
					AdjustmentId = a.AdjustmentId,
					ProductId = a.ProductId,
					ProductName = a.Product.Name,
					ProductBatchId = a.ProductBatchId,
					Type = a.Type.ToString(),
					Reason = a.Reason.ToString(),
					Quantity = a.Quantity,
					TotalFinancialImpact = a.TotalFinancialImpact,
					Note = a.Note,
					CreatedAt = a.CreatedAt
				});
		}

		/// <summary>
		/// Gets a single inventory adjustment by its ID.
		/// </summary>
		[UseFirstOrDefault]
		public IQueryable<AdjustmentDto> GetAdjustmentById(
			Guid adjustmentId,
			[Service] IInventoryDataContext context,
			CancellationToken cancellationToken)
		{
			return context.Adjustments
				.AsNoTracking()
				.Where(a => a.AdjustmentId == adjustmentId)
				.Select(a => new AdjustmentDto
				{
					AdjustmentId = a.AdjustmentId,
					ProductId = a.ProductId,
					ProductName = a.Product.Name,
					ProductBatchId = a.ProductBatchId,
					Type = a.Type.ToString(),
					Reason = a.Reason.ToString(),
					Quantity = a.Quantity,
					TotalFinancialImpact = a.TotalFinancialImpact,
					Note = a.Note,
					CreatedAt = a.CreatedAt
				});
		}

	}
}
