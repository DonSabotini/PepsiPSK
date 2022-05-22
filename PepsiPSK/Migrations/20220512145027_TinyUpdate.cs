using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PepsiPSK.Migrations
{
    public partial class TinyUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCost",
                table: "Orders",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(100,2)",
                oldPrecision: 100,
                oldScale: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCost",
                table: "Orders",
                type: "numeric(100,2)",
                precision: 100,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");
        }
    }
}
