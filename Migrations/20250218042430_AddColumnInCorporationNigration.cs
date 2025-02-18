using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DesktopApplication.Migrations
{
    /// <inheritdoc />
    public partial class AddColumnInCorporationNigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CorporationCreatedOn",
                table: "Corporations",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CorporationCreatedOn",
                table: "Corporations");
        }
    }
}
