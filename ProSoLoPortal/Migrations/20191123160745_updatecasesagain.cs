using Microsoft.EntityFrameworkCore.Migrations;

namespace ProSoLoPortal.Migrations
{
    public partial class updatecasesagain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CaseInformation",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "kussemåtte",
                table: "Case");

            migrationBuilder.AddColumn<string>(
                name: "Bidders",
                table: "Case",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "Case",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsLocked",
                table: "Case",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Case",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfProducts",
                table: "Case",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Seller",
                table: "Case",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TimeFrame",
                table: "Case",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bidders",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "IsLocked",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "NumberOfProducts",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "Seller",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "TimeFrame",
                table: "Case");

            migrationBuilder.AddColumn<string>(
                name: "CaseInformation",
                table: "Case",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "kussemåtte",
                table: "Case",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
