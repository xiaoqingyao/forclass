using Microsoft.EntityFrameworkCore.Migrations;

namespace CoursePlatform.Data.Migrations
{
    public partial class updateLogField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_B_Partner",
                table: "B_Partner");

            migrationBuilder.RenameColumn(
                name: "CouserId",
                table: "B_Course_Audit_Log",
                newName: "CourseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_B_Partner",
                table: "B_Partner",
                columns: new[] { "ResourceId", "Type" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_B_Partner",
                table: "B_Partner");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "B_Course_Audit_Log",
                newName: "CouserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_B_Partner",
                table: "B_Partner",
                column: "IndentityId");
        }
    }
}
