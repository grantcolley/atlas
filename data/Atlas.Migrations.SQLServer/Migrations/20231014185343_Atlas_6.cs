using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Atlas.Migrations.SQLServer.Migrations
{
    /// <inheritdoc />
    public partial class Atlas_6 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComponentArgs");

            migrationBuilder.RenameColumn(
                name: "ComponentCode",
                table: "MenuItems",
                newName: "PageCode");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PageCode",
                table: "MenuItems",
                newName: "ComponentCode");

            migrationBuilder.CreateTable(
                name: "ComponentArgs",
                columns: table => new
                {
                    ComponentArgsId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ComponentCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    ComponentName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    ComponentParameters = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisplayName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RoutingComponentCode = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true),
                    RoutingPage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComponentArgs", x => x.ComponentArgsId);
                    table.CheckConstraint("CK_ComponentArgsId_GreaterThanZero", "ComponentArgsId > 0");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComponentArgs_ComponentName",
                table: "ComponentArgs",
                column: "ComponentName",
                unique: true);
        }
    }
}
