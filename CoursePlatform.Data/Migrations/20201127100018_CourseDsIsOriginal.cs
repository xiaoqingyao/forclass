using Microsoft.EntityFrameworkCore.Migrations;

namespace CoursePlatform.Data.Migrations
{
    public partial class CourseDsIsOriginal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsOriginal",
                table: "B_Course_DS",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOriginal",
                table: "B_Course_DS");
        }
    }
}
