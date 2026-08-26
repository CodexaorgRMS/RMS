using Finance.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Finance.Domain.Entities;

public class JournalEntry
{
    public Guid JournalEntryId { get; set; } = Guid.NewGuid();
    public Guid ReferenceId { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<JournalEntryLine> Lines { get; set; } = new List<JournalEntryLine>();
}
