using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eFlow.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addbouquet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bouquets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    BasePrice = table.Column<decimal>(type: "numeric", nullable: false),
                    Available = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bouquets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BouquetEntityFlowerEntity",
                columns: table => new
                {
                    BouquetsId = table.Column<Guid>(type: "uuid", nullable: false),
                    FlowersId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BouquetEntityFlowerEntity", x => new { x.BouquetsId, x.FlowersId });
                    table.ForeignKey(
                        name: "FK_BouquetEntityFlowerEntity_Bouquets_BouquetsId",
                        column: x => x.BouquetsId,
                        principalTable: "Bouquets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_BouquetEntityFlowerEntity_Flowers_FlowersId",
                        column: x => x.FlowersId,
                        principalTable: "Flowers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BouquetEntityFlowerEntity_FlowersId",
                table: "BouquetEntityFlowerEntity",
                column: "FlowersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BouquetEntityFlowerEntity");

            migrationBuilder.DropTable(
                name: "Bouquets");
        }
    }
}
