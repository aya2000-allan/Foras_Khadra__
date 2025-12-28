using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Foras_Khadra.Migrations
{
    /// <inheritdoc />
    public partial class TeamMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageFileName",
                table: "TeamMembers");

            migrationBuilder.DropColumn(
                name: "IsFounder",
                table: "TeamMembers");

            migrationBuilder.RenameColumn(
                name: "JobTitle",
                table: "TeamMembers",
                newName: "ImagePath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImagePath",
                table: "TeamMembers",
                newName: "JobTitle");

            migrationBuilder.AddColumn<string>(
                name: "ImageFileName",
                table: "TeamMembers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsFounder",
                table: "TeamMembers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
