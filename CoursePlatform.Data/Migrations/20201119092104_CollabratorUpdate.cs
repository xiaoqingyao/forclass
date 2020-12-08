using Microsoft.EntityFrameworkCore.Migrations;

namespace CoursePlatform.Data.Migrations
{
    public partial class CollabratorUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CouserId",
                table: "B_Course_Collabrator");

            migrationBuilder.RenameColumn(
                name: "CollabratorId",
                table: "B_Course_Collabrator",
                newName: "RootId");

            migrationBuilder.AddColumn<string>(
                name: "CourseId",
                table: "B_Course_Collabrator",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ObjId",
                table: "B_Course_Collabrator",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ObjName",
                table: "B_Course_Collabrator",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrgId",
                table: "B_Course_Collabrator",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrgName",
                table: "B_Course_Collabrator",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RootName",
                table: "B_Course_Collabrator",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "B_Course_Collabrator",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CourseId",
                table: "B_Course_Collabrator");

            migrationBuilder.DropColumn(
                name: "ObjId",
                table: "B_Course_Collabrator");

            migrationBuilder.DropColumn(
                name: "ObjName",
                table: "B_Course_Collabrator");

            migrationBuilder.DropColumn(
                name: "OrgId",
                table: "B_Course_Collabrator");

            migrationBuilder.DropColumn(
                name: "OrgName",
                table: "B_Course_Collabrator");

            migrationBuilder.DropColumn(
                name: "RootName",
                table: "B_Course_Collabrator");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "B_Course_Collabrator");

            migrationBuilder.RenameColumn(
                name: "RootId",
                table: "B_Course_Collabrator",
                newName: "CollabratorId");

            migrationBuilder.AddColumn<string>(
                name: "CouserId",
                table: "B_Course_Collabrator",
                type: "nvarchar(100)",
                nullable: true);
        }
    }
}
