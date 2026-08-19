using FluentResults;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Purchases.Application.Features.Suppliers.Commands.CreateSupplier;
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
    }
}
