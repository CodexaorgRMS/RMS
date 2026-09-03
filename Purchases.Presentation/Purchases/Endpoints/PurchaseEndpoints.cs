using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Purchases.Application.Features.Purchases.Commands.Cancel;
using Purchases.Application.Features.Purchases.Commands.Create;
using Purchases.Application.Features.Purchases.Commands.Receive;
using Purchases.Application.Features.Purchases.Commands.Submit;
using Purchases.Application.Features.Purchases.Commands.Update;
using Purchases.Presentation.Requests;
using System;
using System.Linq;
using System.Threading.Tasks;
using Wolverine;
using Wolverine.Http;

namespace Purchases.Presentation.Purchases.Endpoints
{
    public static class PurchaseEndpoints
    {
        [WolverinePost("/api/purchases")]
        public static async Task<IResult> Create(
            CreatePurchaseRequest request,
            [FromHeader(Name = "Idempotency-Key")] string idempotencyKey,
            IMessageBus bus)
        {
            var command = new CreatePurchaseCommand(
                request.SupplierId,
                request.Items
                    .Select(i => new CreatePurchaseItemDto(
                        i.ProductId,
                        i.Quantity,
                        i.UnitCost))
                    .ToList(),
                idempotencyKey);

            var result = await bus.InvokeAsync<Result<CreatePurchaseResult>>(command);

            if (result.IsFailed)
            {
                return Results.Problem(
                    detail: string.Join(", ", result.Errors.Select(e => e.Message)),
                    statusCode: StatusCodes.Status400BadRequest);
            }

            return Results.Created(
                $"/api/purchases/{result.Value.PurchaseOrderId}",
                new
                {
                    purchaseId = result.Value.PurchaseOrderId,
                    status = result.Value.Status
                });
        }

        [WolverinePut("/api/purchases/{purchaseId}")]
        public static async Task<IResult> Update(
            Guid purchaseId,
            UpdatePurchaseRequest request,
            IMessageBus bus)
        {
            var command = new UpdatePurchaseCommand(
                purchaseId,
                request.SupplierId,
                request.Items
                    .Select(i => new UpdatePurchaseItemDto(
                        i.ProductId,
                        i.Quantity,
                        i.UnitCost))
                    .ToList());

            var result = await bus.InvokeAsync<Result<UpdatePurchaseResult>>(command);

            if (result.IsFailed)
            {
                return Results.Problem(
                    detail: string.Join(", ", result.Errors.Select(e => e.Message)),
                    statusCode: StatusCodes.Status400BadRequest);
            }

            return Results.Ok(new
            {
                purchaseId = result.Value.PurchaseOrderId,
                status = result.Value.Status,
                supplierId = result.Value.SupplierId,
                totalAmount = result.Value.TotalAmount,
                items = result.Value.Items
            });
        }

        [WolverinePost("/api/purchases/{purchaseId}/submit")]
        public static async Task<IResult> Submit(
            Guid purchaseId,
            IMessageBus bus)
        {
            var command = new SubmitPurchaseCommand(purchaseId);
            var result = await bus.InvokeAsync<Result<SubmitPurchaseResult>>(command);

            if (result.IsFailed)
            {
                return Results.Problem(
                    detail: string.Join(", ", result.Errors.Select(e => e.Message)),
                    statusCode: StatusCodes.Status400BadRequest);
            }

            return Results.Ok(new
            {
                purchaseId = result.Value.PurchaseOrderId,
                status = result.Value.Status
            });
        }

        [WolverinePost("/api/purchases/{purchaseId}/cancel")]
        public static async Task<IResult> Cancel(
            Guid purchaseId,
            IMessageBus bus)
        {
            var command = new CancelPurchaseCommand(purchaseId);
            var result = await bus.InvokeAsync<Result<CancelPurchaseResult>>(command);

            if (result.IsFailed)
            {
                return Results.Problem(
                    detail: string.Join(", ", result.Errors.Select(e => e.Message)),
                    statusCode: StatusCodes.Status400BadRequest);
            }

            return Results.Ok(new
            {
                purchaseId = result.Value.PurchaseOrderId,
                status = result.Value.Status
            });
        }

        [WolverinePost("/api/purchases/{purchaseId}/receive")]
        public static async Task<IResult> Receive(
            Guid purchaseId,
            ReceivePurchaseRequest request,
            IMessageBus bus)
        {
            var command = new ReceivePurchaseCommand(
                purchaseId,
                request.Items
                    .Select(i => new ReceivePurchaseItemDto(
                        i.PurchaseOrderItemId,
                        i.ReceivedQuantity,
                        i.ExpiryDate))
                    .ToList());

            var result = await bus.InvokeAsync<Result<ReceivePurchaseResult>>(command);

            if (result.IsFailed)
            {
                return Results.Problem(
                    detail: string.Join(", ", result.Errors.Select(e => e.Message)),
                    statusCode: StatusCodes.Status400BadRequest);
            }

            return Results.Ok(new
            {
                purchaseId = result.Value.PurchaseOrderId,
                receiptId = result.Value.ReceiptId,
                status = result.Value.Status
            });
        }
    }
}