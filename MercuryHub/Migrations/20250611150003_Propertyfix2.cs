﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MercuryHub.Migrations
{
    /// <inheritdoc />
    public partial class Propertyfix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HotelId",
                table: "Roles");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HotelId",
                table: "Roles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
