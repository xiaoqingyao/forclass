using Microsoft.EntityFrameworkCore.Migrations;

namespace CoursePlatform.Data.Migrations
{
    public partial class tags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "B_Course",
                newName: "ID");

            migrationBuilder.AddColumn<string>(
                name: "CatalogId",
                table: "B_Course",
                type: "nvarchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CatalogName",
                table: "B_Course",
                type: "nvarchar(300)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CoverUrl",
                table: "B_Course",
                type: "nvarchar(500)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Creator",
                table: "B_Course",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "CreatorName",
                table: "B_Course",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Goal",
                table: "B_Course",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Intro",
                table: "B_Course",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "B_Course",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "RegionName",
                table: "B_Course",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SchoolName",
                table: "B_Course",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Schoolid",
                table: "B_Course",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SignatureId",
                table: "B_Course",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "SignatureName",
                table: "B_Course",
                type: "nvarchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "B_Course",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_B_Course_Deleted",
                table: "B_Course",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "IX_B_Course_ID",
                table: "B_Course",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_B_Course_Deleted",
                table: "B_Course");

            migrationBuilder.DropIndex(
                name: "IX_B_Course_ID",
                table: "B_Course");

            migrationBuilder.DropColumn(
                name: "CatalogId",
                table: "B_Course");

            migrationBuilder.DropColumn(
                name: "CatalogName",
                table: "B_Course");

            migrationBuilder.DropColumn(
                name: "CoverUrl",
                table: "B_Course");

            migrationBuilder.DropColumn(
                name: "Creator",
                table: "B_Course");

            migrationBuilder.DropColumn(
                name: "CreatorName",
                table: "B_Course");

            migrationBuilder.DropColumn(
                name: "Goal",
                table: "B_Course");

            migrationBuilder.DropColumn(
                name: "Intro",
                table: "B_Course");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "B_Course");

            migrationBuilder.DropColumn(
                name: "RegionName",
                table: "B_Course");

            migrationBuilder.DropColumn(
                name: "SchoolName",
                table: "B_Course");

            migrationBuilder.DropColumn(
                name: "Schoolid",
                table: "B_Course");

            migrationBuilder.DropColumn(
                name: "SignatureId",
                table: "B_Course");

            migrationBuilder.DropColumn(
                name: "SignatureName",
                table: "B_Course");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "B_Course");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "B_Course",
                newName: "Id");
        }
    }
}
