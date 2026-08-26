using FluentResults;
using Finance.Application.Abstractions;
using Microsoft.EntityFrameworkCore;
using Wolverine.Attributes;

namespace Finance.Application.Features.Obligations.Commands.CancelObligation;

[Transactional]
public static class CancelObligationCommandHandler
{
    public static async Task<Result> Handle(
        CancelObligationCommand command,
        IFinanceDataContext context,
        CancellationToken cancellationToken)
    {
        var obligation = await context.FinancialObligations
            .FirstOrDefaultAsync(o => o.ObligationId == command.ObligationId, cancellationToken);

        if (obligation is null)
        {
            return Result.Fail("Financial obligation was not found.");
        }

        obligation.Cancel();
        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
