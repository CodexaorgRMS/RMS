using FluentResults;
using Microsoft.EntityFrameworkCore;
using Purchases.Application.Abstractions;
using Purchases.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Wolverine.Attributes;

namespace Purchases.Application.Features.Suppliers.Commands.CreateSupplier
{
    [Transactional(typeof(IPurchasesDataContext))]
    public static class CreateSupplierHandler
    {

        private const string OperationName = "CreateSupplier";
        public static async Task<Result<Guid>> Handle(
        CreateSupplierCommand command,
        IPurchasesDataContext context
        ){
            
            var existingRequest = await context.IdempotencyRecords.AsNoTracking()
            .FirstOrDefaultAsync(x =>
                x.Operation == OperationName &&
                x.IdempotencyKey == command.IdempotencyKey);
                
            if (existingRequest is not null)
            {
                return Result.Ok(existingRequest.ResourceId);
            }

            var supplier = new Supplier
            {
                SupplierId = Guid.NewGuid(),
                Name = command.Name.Trim(),
                Phone = command.Phone?.Trim(),
                Email = command.Email?.Trim(),
                Address = command.Address?.Trim(),
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            var idempotencyRecord = new IdempotencyRecord
            {
                IdempotencyRecordId = Guid.NewGuid(),
                IdempotencyKey = command.IdempotencyKey,
                Operation = OperationName,
                ResourceId = supplier.SupplierId,
                CreatedAt = DateTime.UtcNow
            };
            context.Suppliers.Add(supplier);
            context.IdempotencyRecords.Add(idempotencyRecord);
            return Result.Ok(supplier.SupplierId);
        }
    }
}
