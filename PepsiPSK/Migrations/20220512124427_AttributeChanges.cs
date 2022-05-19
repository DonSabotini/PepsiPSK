using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PepsiPSK.Migrations
{
    public partial class AttributeChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCost",
                table: "Orders",
                type: "numeric(100,2)",
                precision: 100,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(1000,2)",
                oldPrecision: 1000,
                oldScale: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TotalCost",
                table: "Orders",
                type: "numeric(1000,2)",
                precision: 1000,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(100,2)",
                oldPrecision: 100,
                oldScale: 2);
        }
    }
}
