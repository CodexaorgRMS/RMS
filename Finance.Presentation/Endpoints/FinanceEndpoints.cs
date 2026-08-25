using Finance.Application.Features.Obligations.Commands.CancelObligation;
using Finance.Presentation.Mapping;
using Finance.Presentation.Requests;
using FluentResults;
using Microsoft.AspNetCore.Http;
using SharedPresentation.Extentions;
using Wolverine;
using Wolverine.Http;

namespace Finance.Presentation.Endpoints;

public static class FinanceEndpoints
{
    [WolverinePost("/api/finance/shifts/open")]
    public static async Task<IResult> OpenShift(
        OpenShiftRequest request,
        FinanceMapper mapper,
        IMessageBus bus)
    {
        var command = mapper.MapToCommand(request);
        var result = await bus.InvokeAsync<Result<Guid>>(command);

        return result.ToCreatedResult($"/api/finance/shifts/{result.Value}");
    }

    [WolverinePost("/api/finance/shifts/{shiftId}/close")]
    public static async Task<IResult> CloseShift(
        Guid shiftId,
        CloseShiftRequest request,
        FinanceMapper mapper,
        IMessageBus bus)
    {
        var command = mapper.MapToCommand(request, shiftId);
        var result = await bus.InvokeAsync<Result>(command);

        return result.ToHttpResult();
    }

    [WolverinePost("/api/finance/cash-movements")]
    public static async Task<IResult> RecordCashMovement(
        RecordCashMovementRequest request,
        FinanceMapper mapper,
        IMessageBus bus)
    {
        var command = mapper.MapToCommand(request);
        var result = await bus.InvokeAsync<Result<Guid>>(command);

        return result.ToCreatedResult($"/api/finance/cash-movements/{result.Value}");
    }

    [WolverinePost("/api/finance/expenses/categories")]
    public static async Task<IResult> CreateExpenseCategory(
        CreateExpenseCategoryRequest request,
        FinanceMapper mapper,
        IMessageBus bus)
    {
        var command = mapper.MapToCommand(request);
        var result = await bus.InvokeAsync<Result<Guid>>(command);

        return result.ToCreatedResult($"/api/finance/expenses/categories/{result.Value}");
    }

    [WolverinePost("/api/finance/expenses")]
    public static async Task<IResult> RecordExpense(
        RecordExpenseRequest request,
        FinanceMapper mapper,
        IMessageBus bus)
    {
        var command = mapper.MapToCommand(request);
        var result = await bus.InvokeAsync<Result<Guid>>(command);

        return result.ToCreatedResult($"/api/finance/expenses/{result.Value}");
    }

    [WolverinePost("/api/finance/obligations")]
    public static async Task<IResult> CreateObligation(
        CreateObligationRequest request,
        FinanceMapper mapper,
        IMessageBus bus)
    {
        var command = mapper.MapToCommand(request);
        var result = await bus.InvokeAsync<Result<Guid>>(command);

        return result.ToCreatedResult($"/api/finance/obligations/{result.Value}");
    }

    [WolverinePost("/api/finance/obligations/{obligationId}/settle")]
    public static async Task<IResult> SettleObligation(
        Guid obligationId,
        SettleObligationRequest request,
        FinanceMapper mapper,
        IMessageBus bus)
    {
        var command = mapper.MapToCommand(request, obligationId);
        var result = await bus.InvokeAsync<Result>(command);

        return result.ToHttpResult();
    }

    [WolverinePost("/api/finance/obligations/{obligationId}/cancel")]
    public static async Task<IResult> CancelObligation(
        Guid obligationId,
        IMessageBus bus)
    {
        var command = new CancelObligationCommand(obligationId);
        var result = await bus.InvokeAsync<Result>(command);

        return result.ToHttpResult();
    }
}
