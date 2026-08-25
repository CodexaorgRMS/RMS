using FluentResults;
using Finance.Application.Abstractions;
using Finance.Domain.Entities;
using Finance.Domain.Enums;
using Wolverine.Attributes;

namespace Finance.Application.Features.Obligations.Commands.CreateObligation;

[Transactional]
public static class CreateObligationCommandHandler
{
    public static async Task<Result<Guid>> Handle(
        CreateObligationCommand command,
        IFinanceDataContext context,
        CancellationToken cancellationToken)
    {
        var obligation = new FinancialObligation
        {
            Title = command.Title.Trim(),
            Type = command.Type,
            Category = command.Category,
            TotalAmount = command.TotalAmount,
            PaidAmount = 0m,
            RemainingAmount = command.TotalAmount,
            DueDate = command.DueDate,
            Status = ObligationStatus.Pending,
            Notes = command.Notes,
            CreatedAt = DateTime.UtcNow
        };

        context.FinancialObligations.Add(obligation);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok(obligation.ObligationId);
    }
}
