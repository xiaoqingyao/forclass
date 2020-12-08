using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoursePlatform.Data.Migrations
{
    public partial class courseDsCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_B_Partner",
                table: "B_Partner");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "U_platformUser_Course",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "GroupName",
                table: "U_platformUser_Course",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "JoinType",
                table: "U_platformUser_Course",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<string>(
                name: "SchoolName",
                table: "U_PlatformUser",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CourseCount",
                table: "B_Course",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ResearchGroupId",
                table: "B_Course",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ResearchGroupName",
                table: "B_Course",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_B_Partner",
                table: "B_Partner",
                column: "IndentityId");

            migrationBuilder.CreateTable(
                name: "B_Course_Group_Recommend",
                columns: table => new
                {
                    IndentityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    GroupName = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ID = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Deleted = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_B_Course_Group_Recommend", x => x.IndentityId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "B_Course_Group_Recommend");

            migrationBuilder.DropPrimaryKey(
                name: "PK_B_Partner",
                table: "B_Partner");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "U_platformUser_Course");

            migrationBuilder.DropColumn(
                name: "GroupName",
                table: "U_platformUser_Course");

            migrationBuilder.DropColumn(
                name: "JoinType",
                table: "U_platformUser_Course");

            migrationBuilder.DropColumn(
                name: "SchoolName",
                table: "U_PlatformUser");

            migrationBuilder.DropColumn(
                name: "CourseCount",
                table: "B_Course");

            migrationBuilder.DropColumn(
                name: "ResearchGroupId",
                table: "B_Course");

            migrationBuilder.DropColumn(
                name: "ResearchGroupName",
                table: "B_Course");

            migrationBuilder.AddPrimaryKey(
                name: "PK_B_Partner",
                table: "B_Partner",
                columns: new[] { "ResourceId", "Type" });
        }
    }
}
