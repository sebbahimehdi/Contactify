using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ContactManagerPro.Migrations
{
    /// <inheritdoc />
    public partial class AddAdvancedFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Entreprise",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "Favori",
                table: "Contacts",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PhotoUrl",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Profession",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Ville",
                table: "Contacts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Entreprise",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "Favori",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "PhotoUrl",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "Profession",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "Ville",
                table: "Contacts");
        }
    }
}
