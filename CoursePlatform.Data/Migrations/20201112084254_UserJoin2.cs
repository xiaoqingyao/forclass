using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoursePlatform.Data.Migrations
{
    public partial class UserJoin2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "U_platformUser_Course",
                columns: table => new
                {
                    IndentityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Deleted = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    PlatUserId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    CourseId = table.Column<string>(type: "nvarchar(100)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_U_platformUser_Course", x => x.IndentityId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_U_platformUser_Course_CourseId",
                table: "U_platformUser_Course",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_U_platformUser_Course_Deleted",
                table: "U_platformUser_Course",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "IX_U_platformUser_Course_ID",
                table: "U_platformUser_Course",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_U_platformUser_Course_PlatUserId",
                table: "U_platformUser_Course",
                column: "PlatUserId");

            migrationBuilder.CreateIndex(
                name: "IX_U_platformUser_Course_UserId",
                table: "U_platformUser_Course",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "U_platformUser_Course");
        }
    }
}
