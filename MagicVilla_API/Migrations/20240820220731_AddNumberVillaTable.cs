using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class AddNumberVillaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NumberVillas",
                columns: table => new
                {
                    VillaNo = table.Column<int>(type: "int", nullable: false),
                    VillaId = table.Column<int>(type: "int", nullable: false),
                    DetailSpecial = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dateCreate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    dateUpdate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NumberVillas", x => x.VillaNo);
                    table.ForeignKey(
                        name: "FK_NumberVillas_Villas_VillaId",
                        column: x => x.VillaId,
                        principalTable: "Villas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "dateCreate", "dateUpdate" },
                values: new object[] { new DateTime(2024, 8, 20, 19, 7, 28, 986, DateTimeKind.Local).AddTicks(3916), new DateTime(2024, 8, 20, 19, 7, 28, 986, DateTimeKind.Local).AddTicks(3925) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "dateCreate", "dateUpdate" },
                values: new object[] { new DateTime(2024, 8, 20, 19, 7, 28, 986, DateTimeKind.Local).AddTicks(3927), new DateTime(2024, 8, 20, 19, 7, 28, 986, DateTimeKind.Local).AddTicks(3928) });

            migrationBuilder.CreateIndex(
                name: "IX_NumberVillas_VillaId",
                table: "NumberVillas",
                column: "VillaId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NumberVillas");

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "dateCreate", "dateUpdate" },
                values: new object[] { new DateTime(2024, 8, 2, 13, 20, 10, 598, DateTimeKind.Local).AddTicks(3268), new DateTime(2024, 8, 2, 13, 20, 10, 598, DateTimeKind.Local).AddTicks(3282) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "dateCreate", "dateUpdate" },
                values: new object[] { new DateTime(2024, 8, 2, 13, 20, 10, 598, DateTimeKind.Local).AddTicks(3284), new DateTime(2024, 8, 2, 13, 20, 10, 598, DateTimeKind.Local).AddTicks(3285) });
        }
    }
}
