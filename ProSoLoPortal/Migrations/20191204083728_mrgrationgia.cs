using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ProSoLoPortal.Migrations
{
    public partial class mrgrationgia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImagePath",
                table: "Case",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CaseName",
                table: "Bids",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImagePath",
                table: "Case");

            migrationBuilder.DropColumn(
                name: "CaseName",
                table: "Bids");
        }
    }
}
