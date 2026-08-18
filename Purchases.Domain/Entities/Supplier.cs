using System;
using System.Collections.Generic;
using System.Text;

namespace Purchases.Domain.Entities
{
    public class Supplier 
    {
        public Guid SupplierId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string? Phone { get; set; }

        public string? Email { get; set; }

        public string? Address { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
