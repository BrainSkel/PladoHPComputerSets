using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PladoHPComputerSets.Data.Migrations
{
    public partial class addTrackingNR : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OrdererName",
                table: "ComputerOrder",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TrackingNR",
                table: "ComputerOrder",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TrackingNR",
                table: "ComputerOrder");

            migrationBuilder.AlterColumn<string>(
                name: "OrdererName",
                table: "ComputerOrder",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
