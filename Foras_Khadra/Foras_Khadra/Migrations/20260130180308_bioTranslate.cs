using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foras_Khadra.Migrations
{
    /// <inheritdoc />
    public partial class bioTranslate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "TeamMember",
                newName: "NameFr");

            migrationBuilder.RenameColumn(
                name: "MembershipDisplay",
                table: "TeamMember",
                newName: "NameEn");

            migrationBuilder.RenameColumn(
                name: "Bio",
                table: "TeamMember",
                newName: "NameAr");

            migrationBuilder.AddColumn<string>(
                name: "BioAr",
                table: "TeamMember",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BioEn",
                table: "TeamMember",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BioFr",
                table: "TeamMember",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BioAr",
                table: "TeamMember");

            migrationBuilder.DropColumn(
                name: "BioEn",
                table: "TeamMember");

            migrationBuilder.DropColumn(
                name: "BioFr",
                table: "TeamMember");

            migrationBuilder.RenameColumn(
                name: "NameFr",
                table: "TeamMember",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "NameEn",
                table: "TeamMember",
                newName: "MembershipDisplay");

            migrationBuilder.RenameColumn(
                name: "NameAr",
                table: "TeamMember",
                newName: "Bio");
        }
    }
}
