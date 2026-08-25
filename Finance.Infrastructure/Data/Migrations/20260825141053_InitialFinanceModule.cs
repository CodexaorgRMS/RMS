using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Finance.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialFinanceModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Finance");

            migrationBuilder.CreateTable(
                name: "CashMovements",
                schema: "Finance",
                columns: table => new
                {
                    MovementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Source = table.Column<int>(type: "int", nullable: false),
                    ReferenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CashMovements", x => x.MovementId);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseCategories",
                schema: "Finance",
                columns: table => new
                {
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseCategories", x => x.CategoryId);
                });

            migrationBuilder.CreateTable(
                name: "FinancialObligations",
                schema: "Finance",
                columns: table => new
                {
                    ObligationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    PaidAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    RemainingAmount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialObligations", x => x.ObligationId);
                });

            migrationBuilder.CreateTable(
                name: "Shifts",
                schema: "Finance",
                columns: table => new
                {
                    ShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CashierId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OpenedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ClosedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    OpeningFloat = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ActualCashEnd = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    ExpectedCashEnd = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Variance = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shifts", x => x.ShiftId);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                schema: "Finance",
                columns: table => new
                {
                    ExpenseId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Source = table.Column<int>(type: "int", nullable: false),
                    ShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.ExpenseId);
                    table.ForeignKey(
                        name: "FK_Expenses_ExpenseCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalSchema: "Finance",
                        principalTable: "ExpenseCategories",
                        principalColumn: "CategoryId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ObligationSettlements",
                schema: "Finance",
                columns: table => new
                {
                    SettlementId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ObligationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AmountPaid = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Source = table.Column<int>(type: "int", nullable: false),
                    ShiftId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    SettledAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SettledBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObligationSettlements", x => x.SettlementId);
                    table.ForeignKey(
                        name: "FK_ObligationSettlements_FinancialObligations_ObligationId",
                        column: x => x.ObligationId,
                        principalSchema: "Finance",
                        principalTable: "FinancialObligations",
                        principalColumn: "ObligationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CashMovements_CreatedAt",
                schema: "Finance",
                table: "CashMovements",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_CashMovements_ShiftId",
                schema: "Finance",
                table: "CashMovements",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseCategories_Name",
                schema: "Finance",
                table: "ExpenseCategories",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_CategoryId",
                schema: "Finance",
                table: "Expenses",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_CreatedAt",
                schema: "Finance",
                table: "Expenses",
                column: "CreatedAt");

            migrationBuilder.CreateIndex(
                name: "IX_Expenses_ShiftId",
                schema: "Finance",
                table: "Expenses",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialObligations_Category",
                schema: "Finance",
                table: "FinancialObligations",
                column: "Category");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialObligations_DueDate",
                schema: "Finance",
                table: "FinancialObligations",
                column: "DueDate");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialObligations_Status",
                schema: "Finance",
                table: "FinancialObligations",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialObligations_Type",
                schema: "Finance",
                table: "FinancialObligations",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_ObligationSettlements_ObligationId",
                schema: "Finance",
                table: "ObligationSettlements",
                column: "ObligationId");

            migrationBuilder.CreateIndex(
                name: "IX_ObligationSettlements_SettledAt",
                schema: "Finance",
                table: "ObligationSettlements",
                column: "SettledAt");

            migrationBuilder.CreateIndex(
                name: "IX_ObligationSettlements_ShiftId",
                schema: "Finance",
                table: "ObligationSettlements",
                column: "ShiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Shifts_CashierId_Status",
                schema: "Finance",
                table: "Shifts",
                columns: new[] { "CashierId", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CashMovements",
                schema: "Finance");

            migrationBuilder.DropTable(
                name: "Expenses",
                schema: "Finance");

            migrationBuilder.DropTable(
                name: "ObligationSettlements",
                schema: "Finance");

            migrationBuilder.DropTable(
                name: "Shifts",
                schema: "Finance");

            migrationBuilder.DropTable(
                name: "ExpenseCategories",
                schema: "Finance");

            migrationBuilder.DropTable(
                name: "FinancialObligations",
                schema: "Finance");
        }
    }
}
