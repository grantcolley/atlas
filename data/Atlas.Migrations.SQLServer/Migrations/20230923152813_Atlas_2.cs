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
            migrationBuilder.AlterColumn<string>(
                name: "ComponentCode",
                table: "MenuItems",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoutingComponentCode",
                table: "ComponentArgs",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoutingPage",
                table: "ComponentArgs",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RoutingComponentCode",
                table: "ComponentArgs");

            migrationBuilder.DropColumn(
                name: "RoutingPage",
                table: "ComponentArgs");

            migrationBuilder.AlterColumn<string>(
                name: "ComponentCode",
                table: "MenuItems",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);
        }
    }
}
