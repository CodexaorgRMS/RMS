using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Inventory.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddProductBatchFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameTable(
                name: "StockMovements",
                schema: "Inventory",
                newName: "StockMovements");

            migrationBuilder.RenameTable(
                name: "Products",
                schema: "Inventory",
                newName: "Products");

            migrationBuilder.RenameTable(
                name: "ProductBatches",
                schema: "Inventory",
                newName: "ProductBatches");

            migrationBuilder.RenameTable(
                name: "InventoryItems",
                schema: "Inventory",
                newName: "InventoryItems");

            migrationBuilder.RenameTable(
                name: "Categories",
                schema: "Inventory",
                newName: "Categories");

            migrationBuilder.RenameTable(
                name: "Adjustments",
                schema: "Inventory",
                newName: "Adjustments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Inventory");

            migrationBuilder.RenameTable(
                name: "StockMovements",
                newName: "StockMovements",
                newSchema: "Inventory");

            migrationBuilder.RenameTable(
                name: "Products",
                newName: "Products",
                newSchema: "Inventory");

            migrationBuilder.RenameTable(
                name: "ProductBatches",
                newName: "ProductBatches",
                newSchema: "Inventory");

            migrationBuilder.RenameTable(
                name: "InventoryItems",
                newName: "InventoryItems",
                newSchema: "Inventory");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "Categories",
                newSchema: "Inventory");

            migrationBuilder.RenameTable(
                name: "Adjustments",
                newName: "Adjustments",
                newSchema: "Inventory");
        }
    }
}
