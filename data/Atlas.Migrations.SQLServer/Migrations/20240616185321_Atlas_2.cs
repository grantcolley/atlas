using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atlas.Migrations.SQLServer.Migrations
{
    /// <inheritdoc />
    public partial class Atlas_2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsReadOnly",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReadOnly",
                table: "Roles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReadOnly",
                table: "Permissions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReadOnly",
                table: "Modules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReadOnly",
                table: "MenuItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsReadOnly",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsReadOnly",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "IsReadOnly",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "IsReadOnly",
                table: "Permissions");

            migrationBuilder.DropColumn(
                name: "IsReadOnly",
                table: "Modules");

            migrationBuilder.DropColumn(
                name: "IsReadOnly",
                table: "MenuItems");

            migrationBuilder.DropColumn(
                name: "IsReadOnly",
                table: "Categories");
        }
    }
}
