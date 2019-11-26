using Microsoft.EntityFrameworkCore.Migrations;

namespace ProSoLoPortal.Migrations
{
    public partial class updatedcasewithuserforreal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Case",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Case_UserId",
                table: "Case",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Case_AspNetUsers_UserId",
                table: "Case",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Case_AspNetUsers_UserId",
                table: "Case");

            migrationBuilder.DropIndex(
                name: "IX_Case_UserId",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Case");
        }
    }
}
