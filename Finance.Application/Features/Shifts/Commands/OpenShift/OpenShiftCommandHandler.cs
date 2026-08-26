using FluentResults;
using Finance.Application.Abstractions;
using Finance.Domain.Entities;
using Finance.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Wolverine.Attributes;

namespace Finance.Application.Features.Shifts.Commands.OpenShift;

[Transactional]
public static class OpenShiftCommandHandler
{
    public static async Task<Result<Guid>> Handle(
        OpenShiftCommand command,
        IFinanceDataContext context,
        CancellationToken cancellationToken)
    {
        var existingOpenShift = await context.Shifts
            .AnyAsync(s => s.CashierId == command.CashierId && s.Status == ShiftStatus.Open, cancellationToken);

        if (existingOpenShift)
        {
            return Result.Fail<Guid>("Cashier already has an active open shift.");
        }

        var shift = new Shift
        {
            CashierId = command.CashierId,
            OpeningFloat = command.OpeningFloat,
            Notes = command.Notes,
            OpenedAt = DateTime.UtcNow,
            Status = ShiftStatus.Open
        };

        context.Shifts.Add(shift);

        if (command.OpeningFloat > 0)
        {
            var initialMovement = new CashMovement
            {
                ShiftId = shift.ShiftId,
                Amount = command.OpeningFloat,
                Type = CashMovementType.OpeningFloat,
                Description = "Shift Opening Float",
                Source = PaymentSource.Drawer,
                ReferenceId = shift.ShiftId,
                CreatedBy = command.CashierId,
                CreatedAt = shift.OpenedAt
            };

            context.CashMovements.Add(initialMovement);
        }

        await context.SaveChangesAsync(cancellationToken);

        return Result.Ok(shift.ShiftId);
    }
}
