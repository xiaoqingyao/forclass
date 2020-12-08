using Microsoft.EntityFrameworkCore.Migrations;

namespace CoursePlatform.Data.Migrations
{
    public partial class collabratorCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Cover",
                table: "B_Course_DS",
                type: "nvarchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CollbratorCount",
                table: "B_Course",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_B_Course_CollbratorCount",
                table: "B_Course",
                column: "CollbratorCount");

            migrationBuilder.CreateIndex(
                name: "IX_B_Course_CreatorCode",
                table: "B_Course",
                column: "CreatorCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_B_Course_CollbratorCount",
                table: "B_Course");

            migrationBuilder.DropIndex(
                name: "IX_B_Course_CreatorCode",
                table: "B_Course");

            migrationBuilder.DropColumn(
                name: "Cover",
                table: "B_Course_DS");

            migrationBuilder.DropColumn(
                name: "CollbratorCount",
                table: "B_Course");
        }
    }
}
