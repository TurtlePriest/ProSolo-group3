using Microsoft.EntityFrameworkCore.Migrations;

namespace ProSoLoPortal.Migrations
{
    public partial class ibbs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProfileRefId",
                table: "Case",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileRefId",
                table: "Case");
        }
    }
}
