using Finance.Domain.Enums;
using System;

namespace Finance.Domain.Entities;

public class JournalEntryLine
{
    public Guid JournalEntryLineId { get; set; } = Guid.NewGuid();
    public Guid JournalEntryId { get; set; }
    public AccountType AccountType { get; set; }
    public decimal Debit { get; set; }
    public decimal Credit { get; set; }
    public JournalEntry JournalEntry { get; set; } = null!;
}
