using Microsoft.EntityFrameworkCore.Migrations;

namespace ProSoLoPortal.Migrations
{
    public partial class fuckpelle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profile_AspNetUsers_UserId",
                table: "Profile");

            migrationBuilder.AddColumn<int>(
                name: "ProfileRefId",
                table: "Bids",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileRefId",
                table: "Bids");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Profile",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profile_UserId",
                table: "Profile",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Profile_AspNetUsers_UserId",
                table: "Profile",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
