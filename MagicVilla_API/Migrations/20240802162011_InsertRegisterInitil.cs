using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla_API.Migrations
{
    /// <inheritdoc />
    public partial class InsertRegisterInitil : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Amenity", "Description", "ImageUrl", "Name", "Population", "Rate", "SquareMeter", "dateCreate", "dateUpdate" },
                values: new object[,]
                {
                    { 1, "", "Un lugar junto al más que nos permite disfrutar", "", "El mar y tu", 5, 100.0, 230, new DateTime(2024, 8, 2, 13, 20, 10, 598, DateTimeKind.Local).AddTicks(3268), new DateTime(2024, 8, 2, 13, 20, 10, 598, DateTimeKind.Local).AddTicks(3282) },
                    { 2, "", "Un lugar en la sierra, para generar los mejores recuerdos", "", "Momentos serranos", 6, 150.0, 210, new DateTime(2024, 8, 2, 13, 20, 10, 598, DateTimeKind.Local).AddTicks(3284), new DateTime(2024, 8, 2, 13, 20, 10, 598, DateTimeKind.Local).AddTicks(3285) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);
        }
    }
}
