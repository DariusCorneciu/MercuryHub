using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercuryHub.Migrations
{
    /// <inheritdoc />
    public partial class Propertyfix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Properties_Roles_RoleId",
                table: "Properties");

            migrationBuilder.DropIndex(
                name: "IX_Properties_RoleId",
                table: "Properties");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Properties");

            migrationBuilder.AddColumn<int>(
                name: "PropertyId",
                table: "Roles",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Roles_PropertyId",
                table: "Roles",
                column: "PropertyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Roles_Properties_PropertyId",
                table: "Roles",
                column: "PropertyId",
                principalTable: "Properties",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Roles_Properties_PropertyId",
                table: "Roles");

            migrationBuilder.DropIndex(
                name: "IX_Roles_PropertyId",
                table: "Roles");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "Roles");

            migrationBuilder.AddColumn<int>(
                name: "RoleId",
                table: "Properties",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Properties_RoleId",
                table: "Properties",
                column: "RoleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Properties_Roles_RoleId",
                table: "Properties",
                column: "RoleId",
                principalTable: "Roles",
                principalColumn: "Id");
        }
    }
}
