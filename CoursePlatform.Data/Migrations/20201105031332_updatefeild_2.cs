using Microsoft.EntityFrameworkCore.Migrations;

namespace CoursePlatform.Data.Migrations
{
    public partial class updatefeild_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "B_Course");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "B_Course");

            migrationBuilder.DropColumn(
                name: "SchoolId",
                table: "B_Course");

            migrationBuilder.AddColumn<int>(
                name: "CreatorCode",
                table: "B_Course",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RegionCode",
                table: "B_Course",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SchoolCode",
                table: "B_Course",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatorCode",
                table: "B_Course");

            migrationBuilder.DropColumn(
                name: "RegionCode",
                table: "B_Course");

            migrationBuilder.DropColumn(
                name: "SchoolCode",
                table: "B_Course");

            migrationBuilder.AddColumn<int>(
                name: "CreatorId",
                table: "B_Course",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "B_Course",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SchoolId",
                table: "B_Course",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
