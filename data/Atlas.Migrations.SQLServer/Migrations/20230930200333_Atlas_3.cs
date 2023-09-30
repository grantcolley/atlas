using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atlas.Migrations.SQLServer.Migrations
{
    /// <inheritdoc />
    public partial class Atlas_3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NavigateResetBreadcrumb",
                table: "ComponentArgs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NavigateResetBreadcrumb",
                table: "ComponentArgs",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
