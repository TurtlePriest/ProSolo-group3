using Microsoft.EntityFrameworkCore.Migrations;

namespace ProSoLoPortal.Migrations
{
    public partial class ratedupdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rated",
                table: "Bids");

            migrationBuilder.AddColumn<bool>(
                name: "RatedByCus",
                table: "Bids",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RatedByMan",
                table: "Bids",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RatedByCus",
                table: "Bids");

            migrationBuilder.DropColumn(
                name: "RatedByMan",
                table: "Bids");

            migrationBuilder.AddColumn<bool>(
                name: "Rated",
                table: "Bids",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
