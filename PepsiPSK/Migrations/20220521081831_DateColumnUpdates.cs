using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PepsiPSK.Migrations
{
    public partial class DateColumnUpdates : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModificationTime",
                table: "Orders",
                newName: "StatusModificationTime");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModified",
                table: "Flowers",
                type: "timestamp with time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastModified",
                table: "Flowers");

            migrationBuilder.RenameColumn(
                name: "StatusModificationTime",
                table: "Orders",
                newName: "ModificationTime");
        }
    }
}
