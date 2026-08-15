using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class solveissue_baches : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockMovements_ProductBatches_ProductBatchBatchId",
                schema: "Inventory",
                table: "StockMovements");

            migrationBuilder.DropIndex(
                name: "IX_StockMovements_ProductBatchBatchId",
                schema: "Inventory",
                table: "StockMovements");

            migrationBuilder.DropColumn(
                name: "ProductBatchBatchId",
                schema: "Inventory",
                table: "StockMovements");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProductBatchBatchId",
                schema: "Inventory",
                table: "StockMovements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_ProductBatchBatchId",
                schema: "Inventory",
                table: "StockMovements",
                column: "ProductBatchBatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovements_ProductBatches_ProductBatchBatchId",
                schema: "Inventory",
                table: "StockMovements",
                column: "ProductBatchBatchId",
                principalSchema: "Inventory",
                principalTable: "ProductBatches",
                principalColumn: "BatchId");
        }
    }
}
