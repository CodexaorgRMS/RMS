using FluentResults;
using Finance.Application.Abstractions;
using Finance.Domain.Entities;
using Finance.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Wolverine.Attributes;

namespace Finance.Application.Features.CashMovements.Commands.RecordCashMovement;

[Transactional]
public static class RecordCashMovementCommandHandler
{
    public static async Task<Result<Guid>> Handle(
        RecordCashMovementCommand command,
        IFinanceDataContext context,
        CancellationToken cancellationToken)
    {
        if (command.ShiftId.HasValue)
        {
            var shift = await context.Shifts
                .FirstOrDefaultAsync(s => s.ShiftId == command.ShiftId.Value, cancellationToken);

            if (shift is null)
            {
                return Result.Fail<Guid>("Associated shift was not found.");
            }

            if (shift.Status == ShiftStatus.Closed)
            {
                return Result.Fail<Guid>("Cannot record cash movements against a closed shift.");
            }
        }

        var movement = new CashMovement
        {
            ShiftId = command.ShiftId,
            Amount = command.Amount,
            Type = command.Type,
            Description = command.Description,
            Source = command.Source,
            ReferenceId = command.ReferenceId,
            CreatedBy = command.CreatedBy,
            CreatedAt = DateTime.UtcNow
        };

        context.CashMovements.Add(movement);
        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok(movement.MovementId);
    }
}
