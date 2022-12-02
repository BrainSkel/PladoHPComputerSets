using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PladoHPComputerSets.Data.Migrations
{
    public partial class AddComputerOrderReviewsReviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ComputerOrderReview",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    ReviewerName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    ComputerOrderReviewId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComputerOrderReview", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComputerOrderReview_ComputerOrderReview_ComputerOrderReviewId",
                        column: x => x.ComputerOrderReviewId,
                        principalTable: "ComputerOrderReview",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComputerOrderReview_ComputerOrderReviewId",
                table: "ComputerOrderReview",
                column: "ComputerOrderReviewId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ComputerOrderReview");
        }
    }
}
