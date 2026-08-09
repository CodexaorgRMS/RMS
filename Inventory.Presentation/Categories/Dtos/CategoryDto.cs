using System;
using System.Collections.Generic;

namespace Inventory.Presentation.Categories.Dtos
{
    public class CategoryDto
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public Guid? ParentId { get; set; }
        public ParentCategoryDto? Parent { get; set; }
        public ICollection<CategoryDto> Children { get; set; } = new List<CategoryDto>();
    }

    public class ParentCategoryDto
    {
        public Guid CategoryId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
