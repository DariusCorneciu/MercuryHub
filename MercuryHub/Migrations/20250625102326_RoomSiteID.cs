using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercuryHub.Migrations
{
    /// <inheritdoc />
    public partial class RoomSiteID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SiteId",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SiteId",
                table: "Rooms");
        }
    }
}
