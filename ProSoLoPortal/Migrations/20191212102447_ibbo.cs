using Microsoft.EntityFrameworkCore.Migrations;

namespace ProSoLoPortal.Migrations
{
    public partial class ibbo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Seller",
                table: "Case");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Seller",
                table: "Case",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
