using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnsInProductAndCategoryTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CustomAutoMarkdownPercentage",
                table: "Products",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CustomExpiryWarningDays",
                table: "Products",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HoldReason",
                table: "ProductBatches",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "ProductBatches",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<DateTime>(
                name: "StatusChangedAt",
                table: "ProductBatches",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "AutoMarkdownPercentage",
                table: "Categories",
                type: "decimal(5,2)",
                precision: 5,
                scale: 2,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "ExpiryWarningDays",
                table: "Categories",
                type: "int",
                nullable: false,
                defaultValue: 7);

            migrationBuilder.CreateIndex(
                name: "IX_ProductBatch_ProductId_Status_Quantity",
                table: "ProductBatches",
                columns: new[] { "ProductId", "Status", "CurrentQuantity" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ProductBatch_ProductId_Status_Quantity",
                table: "ProductBatches");

            migrationBuilder.DropColumn(
                name: "CustomAutoMarkdownPercentage",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CustomExpiryWarningDays",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "HoldReason",
                table: "ProductBatches");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "ProductBatches");

            migrationBuilder.DropColumn(
                name: "StatusChangedAt",
                table: "ProductBatches");

            migrationBuilder.DropColumn(
                name: "AutoMarkdownPercentage",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ExpiryWarningDays",
                table: "Categories");
        }
    }
}
