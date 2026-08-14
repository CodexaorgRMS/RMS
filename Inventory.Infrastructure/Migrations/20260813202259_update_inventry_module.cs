using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class update_inventry_module : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adjustments_Products_ProductId",
                table: "Adjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Products_ProductId",
                table: "InventoryItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductBatches_Products_ProductId",
                table: "ProductBatches");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMovements_Products_ProductId",
                table: "StockMovements");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItem_ProductId",
                table: "InventoryItems");

            migrationBuilder.DropIndex(
                name: "IX_Adjustment_CreatedBy",
                table: "Adjustments");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Adjustments");

            migrationBuilder.RenameIndex(
                name: "IX_Products_Name",
                table: "Products",
                newName: "IX_Product_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                newName: "IX_Product_CategoryId");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "ProductBatches",
                newName: "InitialQuantity");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_ParentId",
                table: "Categories",
                newName: "IX_Category_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_Name",
                table: "Categories",
                newName: "IX_Category_Name");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReferenceId",
                table: "StockMovements",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ProductBatchBatchId",
                table: "StockMovements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductBatchId",
                table: "StockMovements",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Barcode",
                table: "Products",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "SellingPrice",
                table: "Products",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CostPrice",
                table: "ProductBatches",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "CurrentQuantity",
                table: "ProductBatches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<byte[]>(
                name: "RowVersion",
                table: "ProductBatches",
                type: "rowversion",
                rowVersion: true,
                nullable: false,
                defaultValue: new byte[0]);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Adjustments",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductBatchBatchId",
                table: "Adjustments",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProductBatchId",
                table: "Adjustments",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "Reason",
                table: "Adjustments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "TotalFinancialImpact",
                table: "Adjustments",
                type: "decimal(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_StockMovement_ProductBatchId",
                table: "StockMovements",
                column: "ProductBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovement_ReferenceId",
                table: "StockMovements",
                column: "ReferenceId");

            migrationBuilder.CreateIndex(
                name: "IX_StockMovements_ProductBatchBatchId",
                table: "StockMovements",
                column: "ProductBatchBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Barcode",
                table: "Products",
                column: "Barcode");

            migrationBuilder.CreateIndex(
                name: "IX_Product_IsActive",
                table: "Products",
                column: "IsActive");

            migrationBuilder.CreateIndex(
                name: "IX_ProductBatch_ProductId_CurrentQuantity",
                table: "ProductBatches",
                columns: new[] { "ProductId", "CurrentQuantity" });

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_ProductId",
                table: "InventoryItems",
                column: "ProductId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Adjustment_ProductBatchId",
                table: "Adjustments",
                column: "ProductBatchId");

            migrationBuilder.CreateIndex(
                name: "IX_Adjustment_Reason",
                table: "Adjustments",
                column: "Reason");

            migrationBuilder.CreateIndex(
                name: "IX_Adjustment_Type",
                table: "Adjustments",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_Adjustments_ProductBatchBatchId",
                table: "Adjustments",
                column: "ProductBatchBatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Adjustments_ProductBatches_ProductBatchBatchId",
                table: "Adjustments",
                column: "ProductBatchBatchId",
                principalTable: "ProductBatches",
                principalColumn: "BatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_Adjustments_ProductBatches_ProductBatchId",
                table: "Adjustments",
                column: "ProductBatchId",
                principalTable: "ProductBatches",
                principalColumn: "BatchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Adjustments_Products_ProductId",
                table: "Adjustments",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_Products_ProductId",
                table: "InventoryItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBatches_Products_ProductId",
                table: "ProductBatches",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovements_ProductBatches_ProductBatchBatchId",
                table: "StockMovements",
                column: "ProductBatchBatchId",
                principalTable: "ProductBatches",
                principalColumn: "BatchId");

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovements_ProductBatches_ProductBatchId",
                table: "StockMovements",
                column: "ProductBatchId",
                principalTable: "ProductBatches",
                principalColumn: "BatchId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovements_Products_ProductId",
                table: "StockMovements",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Adjustments_ProductBatches_ProductBatchBatchId",
                table: "Adjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_Adjustments_ProductBatches_ProductBatchId",
                table: "Adjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_Adjustments_Products_ProductId",
                table: "Adjustments");

            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItems_Products_ProductId",
                table: "InventoryItems");

            migrationBuilder.DropForeignKey(
                name: "FK_ProductBatches_Products_ProductId",
                table: "ProductBatches");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMovements_ProductBatches_ProductBatchBatchId",
                table: "StockMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMovements_ProductBatches_ProductBatchId",
                table: "StockMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_StockMovements_Products_ProductId",
                table: "StockMovements");

            migrationBuilder.DropIndex(
                name: "IX_StockMovement_ProductBatchId",
                table: "StockMovements");

            migrationBuilder.DropIndex(
                name: "IX_StockMovement_ReferenceId",
                table: "StockMovements");

            migrationBuilder.DropIndex(
                name: "IX_StockMovements_ProductBatchBatchId",
                table: "StockMovements");

            migrationBuilder.DropIndex(
                name: "IX_Product_Barcode",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Product_IsActive",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_ProductBatch_ProductId_CurrentQuantity",
                table: "ProductBatches");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItem_ProductId",
                table: "InventoryItems");

            migrationBuilder.DropIndex(
                name: "IX_Adjustment_ProductBatchId",
                table: "Adjustments");

            migrationBuilder.DropIndex(
                name: "IX_Adjustment_Reason",
                table: "Adjustments");

            migrationBuilder.DropIndex(
                name: "IX_Adjustment_Type",
                table: "Adjustments");

            migrationBuilder.DropIndex(
                name: "IX_Adjustments_ProductBatchBatchId",
                table: "Adjustments");

            migrationBuilder.DropColumn(
                name: "ProductBatchBatchId",
                table: "StockMovements");

            migrationBuilder.DropColumn(
                name: "ProductBatchId",
                table: "StockMovements");

            migrationBuilder.DropColumn(
                name: "Barcode",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SellingPrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CostPrice",
                table: "ProductBatches");

            migrationBuilder.DropColumn(
                name: "CurrentQuantity",
                table: "ProductBatches");

            migrationBuilder.DropColumn(
                name: "RowVersion",
                table: "ProductBatches");

            migrationBuilder.DropColumn(
                name: "ProductBatchBatchId",
                table: "Adjustments");

            migrationBuilder.DropColumn(
                name: "ProductBatchId",
                table: "Adjustments");

            migrationBuilder.DropColumn(
                name: "Reason",
                table: "Adjustments");

            migrationBuilder.DropColumn(
                name: "TotalFinancialImpact",
                table: "Adjustments");

            migrationBuilder.RenameIndex(
                name: "IX_Product_Name",
                table: "Products",
                newName: "IX_Products_Name");

            migrationBuilder.RenameIndex(
                name: "IX_Product_CategoryId",
                table: "Products",
                newName: "IX_Products_CategoryId");

            migrationBuilder.RenameColumn(
                name: "InitialQuantity",
                table: "ProductBatches",
                newName: "Quantity");

            migrationBuilder.RenameIndex(
                name: "IX_Category_ParentId",
                table: "Categories",
                newName: "IX_Categories_ParentId");

            migrationBuilder.RenameIndex(
                name: "IX_Category_Name",
                table: "Categories",
                newName: "IX_Categories_Name");

            migrationBuilder.AlterColumn<Guid>(
                name: "ReferenceId",
                table: "StockMovements",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Adjustments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<int>(
                name: "CreatedBy",
                table: "Adjustments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_ProductId",
                table: "InventoryItems",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_Adjustment_CreatedBy",
                table: "Adjustments",
                column: "CreatedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_Adjustments_Products_ProductId",
                table: "Adjustments",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItems_Products_ProductId",
                table: "InventoryItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProductBatches_Products_ProductId",
                table: "ProductBatches",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StockMovements_Products_ProductId",
                table: "StockMovements",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ProductId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
