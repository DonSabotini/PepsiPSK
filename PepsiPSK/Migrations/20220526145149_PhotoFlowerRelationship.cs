using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PepsiPSK.Migrations
{
    public partial class PhotoFlowerRelationship : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoLink",
                table: "Flowers");

            migrationBuilder.AddColumn<Guid>(
                name: "PhotoId",
                table: "Flowers",
                type: "uuid",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PhotoId",
                table: "Flowers");

            migrationBuilder.AddColumn<string>(
                name: "PhotoLink",
                table: "Flowers",
                type: "text",
                nullable: true);
        }
    }
}
