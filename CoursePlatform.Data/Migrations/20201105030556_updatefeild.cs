using Microsoft.EntityFrameworkCore.Migrations;

namespace CoursePlatform.Data.Migrations
{
    public partial class updatefeild : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Creator",
                table: "B_Course");

            migrationBuilder.RenameColumn(
                name: "Schoolid",
                table: "B_Course",
                newName: "SchoolId");

            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "B_Course",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "B_Course");

            migrationBuilder.RenameColumn(
                name: "SchoolId",
                table: "B_Course",
                newName: "Schoolid");

            migrationBuilder.AddColumn<int>(
                name: "Creator",
                table: "B_Course",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
