using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PepsiPSK.Migrations
{
    public partial class RemoveFlowerOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flowers_Orders_OrderId",
                table: "Flowers");

            migrationBuilder.DropIndex(
                name: "IX_Flowers_OrderId",
                table: "Flowers");

            migrationBuilder.DropColumn(
                name: "OrderId",
                table: "Flowers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "OrderId",
                table: "Flowers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Flowers_OrderId",
                table: "Flowers",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flowers_Orders_OrderId",
                table: "Flowers",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id");
        }
    }
}
