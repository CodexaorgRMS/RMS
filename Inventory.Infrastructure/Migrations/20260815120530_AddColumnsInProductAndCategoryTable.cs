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
				schema: "Inventory",
				table: "Products",
				type: "decimal(5,2)",
				precision: 5,
				scale: 2,
				nullable: true);

			migrationBuilder.AddColumn<int>(
				name: "CustomExpiryWarningDays",
				schema: "Inventory",
				table: "Products",
				type: "int",
				nullable: true);

			migrationBuilder.AddColumn<string>(
				name: "HoldReason",
				schema: "Inventory",
				table: "ProductBatches",
				type: "nvarchar(500)",
				maxLength: 500,
				nullable: true);

			migrationBuilder.AddColumn<int>(
				name: "Status",
				schema: "Inventory",
				table: "ProductBatches",
				type: "int",
				nullable: false,
				defaultValue: 1);

			migrationBuilder.AddColumn<DateTime>(
				name: "StatusChangedAt",
				schema: "Inventory",
				table: "ProductBatches",
				type: "datetime2",
				nullable: true);

			migrationBuilder.AddColumn<decimal>(
				name: "AutoMarkdownPercentage",
				schema: "Inventory",
				table: "Categories",
				type: "decimal(5,2)",
				precision: 5,
				scale: 2,
				nullable: false,
				defaultValue: 0m);

			migrationBuilder.AddColumn<int>(
				name: "ExpiryWarningDays",
				schema: "Inventory",
				table: "Categories",
				type: "int",
				nullable: false,
				defaultValue: 7);

			migrationBuilder.CreateIndex(
				name: "IX_ProductBatch_ProductId_Status_Quantity",
				schema: "Inventory",
				table: "ProductBatches",
				columns: new[]
				{
					"ProductId",
					"Status",
					"CurrentQuantity"
				});
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropIndex(
				name: "IX_ProductBatch_ProductId_Status_Quantity",
				schema: "Inventory",
				table: "ProductBatches");

			migrationBuilder.DropColumn(
				name: "CustomAutoMarkdownPercentage",
				schema: "Inventory",
				table: "Products");

			migrationBuilder.DropColumn(
				name: "CustomExpiryWarningDays",
				schema: "Inventory",
				table: "Products");

			migrationBuilder.DropColumn(
				name: "HoldReason",
				schema: "Inventory",
				table: "ProductBatches");

			migrationBuilder.DropColumn(
				name: "Status",
				schema: "Inventory",
				table: "ProductBatches");

			migrationBuilder.DropColumn(
				name: "StatusChangedAt",
				schema: "Inventory",
				table: "ProductBatches");

			migrationBuilder.DropColumn(
				name: "AutoMarkdownPercentage",
				schema: "Inventory",
				table: "Categories");

			migrationBuilder.DropColumn(
				name: "ExpiryWarningDays",
				schema: "Inventory",
				table: "Categories");
		}
	}
}