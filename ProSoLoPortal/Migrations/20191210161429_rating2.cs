using Microsoft.EntityFrameworkCore.Migrations;

namespace ProSoLoPortal.Migrations
{
    public partial class rating2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rating",
                columns: table => new
                {
                    RatingId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProfileRefId = table.Column<int>(nullable: false),
                    RatingNum = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rating", x => x.RatingId);
                    table.ForeignKey(
                        name: "FK_Rating_Profile_ProfileRefId",
                        column: x => x.ProfileRefId,
                        principalTable: "Profile",
                        principalColumn: "ProfileId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rating_ProfileRefId",
                table: "Rating",
                column: "ProfileRefId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rating");
        }
    }
}
