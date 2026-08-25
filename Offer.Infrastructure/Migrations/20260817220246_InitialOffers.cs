using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Offers.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialOffers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Offers");

            migrationBuilder.CreateTable(
                name: "Offers",
                schema: "Offers",
                columns: table => new
                {
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Value = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsSmart = table.Column<bool>(type: "bit", nullable: false),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Offers", x => x.OfferId);
                });

            migrationBuilder.CreateTable(
                name: "OfferTargets",
                schema: "Offers",
                columns: table => new
                {
                    OfferTargetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OfferId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TargetType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TargetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequiredQuantity = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    SpecialPrice = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OfferTargets", x => x.OfferTargetId);
                    table.ForeignKey(
                        name: "FK_OfferTargets_Offers_OfferId",
                        column: x => x.OfferId,
                        principalSchema: "Offers",
                        principalTable: "Offers",
                        principalColumn: "OfferId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Offer_Active_Dates",
                schema: "Offers",
                table: "Offers",
                columns: new[] { "IsActive", "StartDate", "EndDate" });

            migrationBuilder.CreateIndex(
                name: "IX_Offer_Priority",
                schema: "Offers",
                table: "Offers",
                column: "Priority");

            migrationBuilder.CreateIndex(
                name: "IX_Offer_Type",
                schema: "Offers",
                table: "Offers",
                column: "Type");

            migrationBuilder.CreateIndex(
                name: "IX_OfferTarget_Offer_Type_Target",
                schema: "Offers",
                table: "OfferTargets",
                columns: new[] { "OfferId", "TargetType", "TargetId" });

            migrationBuilder.CreateIndex(
                name: "IX_OfferTarget_TargetId",
                schema: "Offers",
                table: "OfferTargets",
                column: "TargetId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OfferTargets",
                schema: "Offers");

            migrationBuilder.DropTable(
                name: "Offers",
                schema: "Offers");
        }
    }
}
