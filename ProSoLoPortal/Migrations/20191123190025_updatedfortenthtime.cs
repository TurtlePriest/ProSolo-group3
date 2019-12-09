using Microsoft.EntityFrameworkCore.Migrations;

namespace ProSoLoPortal.Migrations
{
    public partial class updatedfortenthtime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Bidders",
                table: "Case");

            migrationBuilder.CreateTable(
                name: "Bidders",
                columns: table => new
                {
                    ID = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Checked = table.Column<bool>(nullable: false),
                    CaseId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bidders", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Bidders_Case_CaseId",
                        column: x => x.CaseId,
                        principalTable: "Case",
                        principalColumn: "CaseId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bidders_CaseId",
                table: "Bidders",
                column: "CaseId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bidders");

            migrationBuilder.AddColumn<string>(
                name: "Bidders",
                table: "Case",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
