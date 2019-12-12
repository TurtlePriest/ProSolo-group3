using Microsoft.EntityFrameworkCore.Migrations;

namespace ProSoLoPortal.Migrations
{
    public partial class yeeeeet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ManufacturerId",
                table: "Case",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Case_ManufacturerId",
                table: "Case",
                column: "ManufacturerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Case_AspNetUsers_ManufacturerId",
                table: "Case",
                column: "ManufacturerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Case_AspNetUsers_ManufacturerId",
                table: "Case");

            migrationBuilder.DropIndex(
                name: "IX_Case_ManufacturerId",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "ManufacturerId",
                table: "Case");
        }
    }
}
