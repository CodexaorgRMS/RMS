using Finance.Application.Features.CashMovements.Commands.RecordCashMovement;
using Finance.Application.Features.Expenses.Commands.CreateExpenseCategory;
using Finance.Application.Features.Expenses.Commands.RecordExpense;
using Finance.Application.Features.Obligations.Commands.CreateObligation;
using Finance.Application.Features.Obligations.Commands.SettleObligation;
using Finance.Application.Features.Shifts.Commands.CloseShift;
using Finance.Application.Features.Shifts.Commands.OpenShift;
using Finance.Domain.Enums;
using Finance.Presentation.Requests;
using Riok.Mapperly.Abstractions;

namespace Finance.Presentation.Mapping;

[Mapper]
public sealed partial class FinanceMapper
{
    public partial OpenShiftCommand MapToCommand(OpenShiftRequest request);

    public CloseShiftCommand MapToCommand(CloseShiftRequest request, Guid shiftId)
    {
        return new CloseShiftCommand(shiftId, request.ActualCash, request.Notes);
    }

    public RecordCashMovementCommand MapToCommand(RecordCashMovementRequest request)
    {
        return new RecordCashMovementCommand(
            request.ShiftId,
            request.Amount,
            (CashMovementType)request.Type,
            request.Description,
            (PaymentSource)request.Source,
            request.ReferenceId,
            request.CreatedBy);
    }

    public partial CreateExpenseCategoryCommand MapToCommand(CreateExpenseCategoryRequest request);

    public RecordExpenseCommand MapToCommand(RecordExpenseRequest request)
    {
        return new RecordExpenseCommand(
            request.CategoryId,
            request.Amount,
            request.Description,
            (PaymentSource)request.Source,
            request.ShiftId,
            request.CreatedBy);
    }

    public CreateObligationCommand MapToCommand(CreateObligationRequest request)
    {
        return new CreateObligationCommand(
            request.Title,
            (ObligationType)request.Type,
            (ObligationCategory)request.Category,
            request.TotalAmount,
            request.DueDate,
            request.Notes);
    }

    public SettleObligationCommand MapToCommand(SettleObligationRequest request, Guid obligationId)
    {
        return new SettleObligationCommand(
            obligationId,
            request.Amount,
            (PaymentSource)request.Source,
            request.ShiftId,
            request.Notes,
            request.SettledBy);
    }
}
