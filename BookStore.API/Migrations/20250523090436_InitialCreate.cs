using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStore.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "Books",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Author = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Stock = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ISBN = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedDate = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Books", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "Author", "CreatedDate", "Description", "ISBN", "Price", "Stock", "Title" },
                values: new object[,]
                {
                    { 1, "Andrzej Sapkowski", new DateTime(2025, 5, 23, 11, 4, 35, 757, DateTimeKind.Local).AddTicks(7624), "Pierwszy tom kultowej sagi o wiedźminie", "9788375780635", 39.99m, 10, "Wiedźmin: Ostatnie życzenie" },
                    { 2, "J.R.R. Tolkien", new DateTime(2025, 5, 23, 11, 4, 35, 757, DateTimeKind.Local).AddTicks(7631), "Klasyczna powieść fantasy", "9788324159819", 49.99m, 5, "Hobbit, czyli tam i z powrotem" },
                    { 3, "George Orwell", new DateTime(2025, 5, 23, 11, 4, 35, 757, DateTimeKind.Local).AddTicks(7637), "Rok 1984 w wersji orwellowskiej", "9788382022287", 29.99m, 8, "1984" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Books");
        }
    }
}
