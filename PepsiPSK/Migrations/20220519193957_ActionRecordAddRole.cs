using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PepsiPSK.Migrations
{
    public partial class ActionRecordAddRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "ActionRecords",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "ActionRecords");
        }
    }
}
