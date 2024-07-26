using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eFlow.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class addmedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ImageFileId",
                table: "Flowers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MediaFileEntity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FileName = table.Column<string>(type: "text", nullable: false),
                    FilePath = table.Column<string>(type: "text", nullable: false),
                    ContentType = table.Column<string>(type: "text", nullable: false),
                    Size = table.Column<long>(type: "bigint", nullable: false),
                    BouquetSizeEntityId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFileEntity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaFileEntity_BouquetSizeEntity_BouquetSizeEntityId",
                        column: x => x.BouquetSizeEntityId,
                        principalTable: "BouquetSizeEntity",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flowers_ImageFileId",
                table: "Flowers",
                column: "ImageFileId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaFileEntity_BouquetSizeEntityId",
                table: "MediaFileEntity",
                column: "BouquetSizeEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flowers_MediaFileEntity_ImageFileId",
                table: "Flowers",
                column: "ImageFileId",
                principalTable: "MediaFileEntity",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flowers_MediaFileEntity_ImageFileId",
                table: "Flowers");

            migrationBuilder.DropTable(
                name: "MediaFileEntity");

            migrationBuilder.DropIndex(
                name: "IX_Flowers_ImageFileId",
                table: "Flowers");

            migrationBuilder.DropColumn(
                name: "ImageFileId",
                table: "Flowers");
        }
    }
}
