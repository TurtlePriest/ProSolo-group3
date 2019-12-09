using Microsoft.EntityFrameworkCore.Migrations;

namespace ProSoLoPortal.Migrations
{
    public partial class updatedstuff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropColumn(
                name: "UserRefId",
                table: "Case");

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "Case",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EmployeeId",
                table: "Case",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Case_CustomerId",
                table: "Case",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Case_EmployeeId",
                table: "Case",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Case_AspNetUsers_CustomerId",
                table: "Case",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Case_AspNetUsers_EmployeeId",
                table: "Case",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Case_AspNetUsers_CustomerId",
                table: "Case");

            migrationBuilder.DropForeignKey(
                name: "FK_Case_AspNetUsers_EmployeeId",
                table: "Case");

            migrationBuilder.DropIndex(
                name: "IX_Case_CustomerId",
                table: "Case");

            migrationBuilder.DropIndex(
                name: "IX_Case_EmployeeId",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "Case");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Case",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserRefId",
                table: "Case",
                type: "nvarchar(max)",
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
    }
}
