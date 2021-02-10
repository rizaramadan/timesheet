using Microsoft.EntityFrameworkCore.Migrations;

namespace Timesheet.Migrations
{
    public partial class grouporder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "ActivityGroups",
                type: "integer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                table: "ActivityGroups");
        }
    }
}
