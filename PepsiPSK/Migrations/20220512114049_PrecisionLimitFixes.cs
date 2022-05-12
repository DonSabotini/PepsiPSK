using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PepsiPSK.Migrations
{
    public partial class PrecisionLimitFixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCost",
                table: "Orders",
                type: "numeric(1000,2)",
                precision: 1000,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Flowers",
                type: "numeric(6,2)",
                precision: 6,
                scale: 2,
                nullable: false,
                oldClrType: typeof(double),
                oldType: "double precision");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "TotalCost",
                table: "Orders",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(1000,2)",
                oldPrecision: 1000,
                oldScale: 2);

            migrationBuilder.AlterColumn<double>(
                name: "Price",
                table: "Flowers",
                type: "double precision",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(6,2)",
                oldPrecision: 6,
                oldScale: 2);
        }
    }
}
