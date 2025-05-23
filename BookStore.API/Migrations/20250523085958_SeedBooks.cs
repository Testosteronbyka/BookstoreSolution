using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStore.API.Migrations
{
    /// <inheritdoc />
    public partial class SeedBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "CreatedDate", "Description", "ISBN", "Price", "Stock", "Title" },
                values: new object[,]
                {
                    { 1, "Andrzej Sapkowski", new DateTime(2025, 5, 23, 10, 59, 58, 181, DateTimeKind.Local).AddTicks(7242), "Pierwszy tom kultowej sagi o wiedźminie", "9788375780635", 39.99m, 10, "Wiedźmin: Ostatnie życzenie" },
                    { 2, "J.R.R. Tolkien", new DateTime(2025, 5, 23, 10, 59, 58, 181, DateTimeKind.Local).AddTicks(7247), "Klasyczna powieść fantasy", "9788324159819", 49.99m, 5, "Hobbit, czyli tam i z powrotem" },
                    { 3, "George Orwell", new DateTime(2025, 5, 23, 10, 59, 58, 181, DateTimeKind.Local).AddTicks(7252), "Rok 1984 w wersji orwellowskiej", "9788382022287", 29.99m, 8, "1984" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
