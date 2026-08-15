using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using Inventory.Application.Abstractions;
using Inventory.Presentation.Categories.Dtos;
using Inventory.Presentation.Shared;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Presentation.Categories.Queries
{
	[ExtendObjectType(typeof(Query))]
	public class CategoryQueries
    {
        [UsePaging(IncludeTotalCount = true)]
        [UseFiltering]
        [UseSorting]
        public IQueryable<CategoryDto> GetCategories([Service] IInventoryDataContext context)
        {
            return context.Categories
                .Include(c => c.Parent)
				.Select(c => new CategoryDto
            {
                CategoryId = c.CategoryId,
                Name = c.Name,
                Description = c.Description,
                ParentId = c.ParentId,
                Parent = c.Parent != null ? new ParentCategoryDto 
                { 
                    CategoryId = c.Parent.CategoryId, 
                    Name = c.Parent.Name, 
                    Description = c.Parent.Description 
                } : null
            });
        }

        public async Task< CategoryDto?> GetCategoryById([Service] IInventoryDataContext context, Guid categoryId)
        {
            return await context.Categories
                .Include(c => c.Parent)
				.Where(c => c.CategoryId == categoryId)
                .Select(c => new CategoryDto
                {
                    CategoryId = c.CategoryId,
                    Name = c.Name,
                    Description = c.Description,
                    ParentId = c.ParentId,
                    Parent = c.Parent != null ? new ParentCategoryDto 
                    { 
                        CategoryId = c.Parent.CategoryId, 
                        Name = c.Parent.Name, 
                        Description = c.Parent.Description 
                    } : null
                })
                .FirstOrDefaultAsync();
        }

		public async Task<IEnumerable<CategoryDto>> GetCategoryTree(
	[Service] IInventoryDataContext context,
	CancellationToken cancellationToken,
	Guid? rootId = null,
	int skip = 0,
	int take = 10,
	int maxDepth = 2) 
		{
			if (skip < 0) skip = 0;
			if (take <= 0) take = 10;
			if (take > 100) take = 100;
			if (maxDepth < 0) maxDepth = 0;
			if (maxDepth > 10) maxDepth = 10; 

	
			var rootsQuery = context.Categories.AsNoTracking();
			rootsQuery = rootId.HasValue
				? rootsQuery.Where(c => c.CategoryId == rootId.Value)
				: rootsQuery.Where(c => c.ParentId == null);

			var pagedRoots = await rootsQuery
				.OrderBy(c => c.Name)
				.Skip(skip)
				.Take(take)
				.Select(c => new CategoryDto
				{
					CategoryId = c.CategoryId,
					Name = c.Name,
					Description = c.Description,
					ParentId = c.ParentId
				})
				.ToListAsync(cancellationToken);

			if (pagedRoots.Count == 0 || maxDepth == 0)
			{
				return pagedRoots;
			}

	
			var allDtos = new Dictionary<Guid, CategoryDto>();
			foreach (var r in pagedRoots) allDtos[r.CategoryId] = r;

			var currentLevelIds = pagedRoots.Select(r => r.CategoryId).ToList();

			for (int level = 0; level < maxDepth && currentLevelIds.Count > 0; level++)
			{
				var nextLevel = await context.Categories
					.AsNoTracking()
					.Where(c => c.ParentId != null && currentLevelIds.Contains(c.ParentId.Value))
					.Select(c => new CategoryDto
					{
						CategoryId = c.CategoryId,
						Name = c.Name,
						Description = c.Description,
						ParentId = c.ParentId
					})
					.ToListAsync(cancellationToken);

				if (nextLevel.Count == 0) break;

				foreach (var dto in nextLevel)
				{
					allDtos[dto.CategoryId] = dto;
				}

				currentLevelIds = nextLevel.Select(c => c.CategoryId).ToList();
			}

			var lookup = allDtos.Values.ToLookup(c => c.ParentId);
			foreach (var root in pagedRoots)
			{
				AssignChildren(root, lookup);
			}

			return pagedRoots;
		}

		private static void AssignChildren(CategoryDto current, ILookup<Guid?, CategoryDto> lookup)
		{
			var children = lookup[current.CategoryId].ToList();
			current.Children = children;
			foreach (var child in children)
			{
				AssignChildren(child, lookup);
			}
		}
	}
}
