using FluentResults;
using Finance.Application.Abstractions;
using Finance.Domain.Entities;
using Finance.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using SharedContracts.Finance.Events;
using Wolverine;
using Wolverine.Attributes;

namespace Finance.Application.Features.Obligations.Commands.SettleObligation;

[Transactional]
public static class SettleObligationCommandHandler
{
    public static async Task<Result> Handle(
        SettleObligationCommand command,
        IFinanceDataContext context,
        IMessageBus bus,
        CancellationToken cancellationToken)
    {
        var obligation = await context.FinancialObligations
            .FirstOrDefaultAsync(o => o.ObligationId == command.ObligationId, cancellationToken);

        if (obligation is null)
        {
            return Result.Fail("Financial obligation was not found.");
        }

        if (obligation.Status == ObligationStatus.Cancelled)
        {
            return Result.Fail("Cannot settle a cancelled obligation.");
        }

        if (obligation.Status == ObligationStatus.FullySettled)
        {
            return Result.Fail("Obligation is already fully settled.");
        }

        if (command.Amount > obligation.RemainingAmount)
        {
            return Result.Fail($"Settlement amount ({command.Amount}) exceeds remaining balance ({obligation.RemainingAmount}).");
        }

        if (command.ShiftId.HasValue)
        {
            var shift = await context.Shifts
                .FirstOrDefaultAsync(s => s.ShiftId == command.ShiftId.Value, cancellationToken);

            if (shift is null)
            {
                return Result.Fail("Associated shift was not found.");
            }

            if (shift.Status == ShiftStatus.Closed)
            {
                return Result.Fail("Cannot settle an obligation against a closed shift.");
            }
        }

        obligation.Settle(command.Amount);

        var settlement = new ObligationSettlement
        {
            ObligationId = obligation.ObligationId,
            AmountPaid = command.Amount,
            Source = command.Source,
            ShiftId = command.ShiftId,
            Notes = command.Notes,
            SettledAt = DateTime.UtcNow,
            SettledBy = command.SettledBy
        };

        context.ObligationSettlements.Add(settlement);

        var cashMovement = new CashMovement
        {
            ShiftId = command.ShiftId,
            Amount = command.Amount,
            Type = CashMovementType.ObligationSettlement,
            Description = obligation.Type == ObligationType.Payable
                ? $"Payable Settlement: {obligation.Title}"
                : $"Receivable Collection: {obligation.Title}",
            Source = command.Source,
            ReferenceId = settlement.SettlementId,
            CreatedBy = command.SettledBy,
            CreatedAt = settlement.SettledAt
        };

        context.CashMovements.Add(cashMovement);

        await bus.PublishAsync(new ObligationSettledIntegrationEvent(
            obligation.ObligationId,
            command.Amount,
            obligation.RemainingAmount,
            command.Source.ToString(),
            DateTime.UtcNow));

        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok();
    }
}
