using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Purchases.Application.Features.Suppliers.Commands.ActivateSupplier;
using Purchases.Application.Features.Suppliers.Commands.CreateSupplier;
using Purchases.Application.Features.Suppliers.Commands.DeactivateSupplier;
using Purchases.Application.Features.Suppliers.Commands.UpdateSupplier;
using Purchases.Presentation.Requests;
using Purchases.Presentation.Suppliers.Mappers;
using System;
using System.Collections.Generic;
using System.Text;
using Wolverine;
using Wolverine.Http;

namespace Purchases.Presentation.Suppliers.Endpoints
{
    public static class SupplierEndpoints
    {
        [WolverinePost("/api/suppliers")]
        public static async Task<IResult> Create(
        CreateSupplierRequest request,
        [FromServices] SupplierMapper mapper,
         [FromHeader(Name = "Idempotency-Key")] string idempotencyKey,
        IMessageBus _bus
        )
        {
            var command = new CreateSupplierCommand(
                 request.Name,
                 request.Phone,
                 request.Email,
                 request.Address,
                 idempotencyKey);

            var result = await _bus.InvokeAsync<Result<Guid>>(command);

            if (result.IsFailed)
            {
                return Results.Problem(

                detail: string.Join(
                ", ",
                result.Errors.Select(e => e.Message),
                StatusCodes.Status400BadRequest
                ));
            }

            return Results.Created(
                       $"/api/suppliers/{result.Value}",
                       result.Value);
        }

        [WolverinePut("/api/suppliers/{supplierId}")]
        public static async Task<IResult> Update(
         Guid supplierId,
         UpdateSupplierRequest request,
         IMessageBus bus)
        {
            var command = new UpdateSupplierCommand(
                supplierId,
                request.Name,
                request.Phone,
                request.Email,
                request.Address);

            var result = await bus.InvokeAsync<Result>(command);

            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.NoContent();
        }

        [WolverinePost("/api/suppliers/{supplierId}/deactivate")]
        public static async Task<IResult> Deactivate(
          Guid supplierId,
          IMessageBus bus)
        {
            var command = new DeactivateSupplierCommand(supplierId);

            var result = await bus.InvokeAsync<Result>(command);

            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.NoContent();
        }

        [WolverinePost("/api/suppliers/{supplierId}/activate")]
        public static async Task<IResult> Activate(
          Guid supplierId,
          IMessageBus bus)
        {
            var command = new ActivateSupplierCommand(supplierId);

            var result = await bus.InvokeAsync<Result>(command);

            if (result.IsFailed)
            {
                return Results.BadRequest(result.Errors);
            }

            return Results.NoContent();
        }
    }
}
