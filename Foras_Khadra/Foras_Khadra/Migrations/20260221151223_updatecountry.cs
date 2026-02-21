using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Foras_Khadra.Migrations
{
    /// <inheritdoc />
    public partial class updatecountry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "Id", "NameAr", "NameEn", "NameFr" },
                values: new object[,]
                {
                    { 23, "الوطن العربي ", "The Arab World", "Le Monde arabe" },
                    { 24, "كل دول العالم", "All World", "Tous les pays du monde" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "Countries",
                keyColumn: "Id",
                keyValue: 24);
        }
    }
}
