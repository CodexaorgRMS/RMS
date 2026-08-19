using System;
using System.Collections.Generic;
using System.Text;

namespace Purchases.Domain.Entities
{
    public class IdempotencyRecord
    {
        public Guid IdempotencyRecordId { get; set; }

        public string IdempotencyKey { get; set; } = string.Empty;

        public string Operation { get; set; } = string.Empty;

        public Guid ResourceId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
