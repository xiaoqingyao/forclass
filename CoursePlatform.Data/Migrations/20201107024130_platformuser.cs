using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoursePlatform.Data.Migrations
{
    public partial class platformuser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "U_PlatformUser",
                columns: table => new
                {
                    IndentityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Deleted = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    UserId = table.Column<int>(nullable: false),
                    SchoolId = table.Column<int>(nullable: false),
                    SectionId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CourseShelves = table.Column<int>(nullable: false),
                    StdJoined = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_U_PlatformUser", x => x.IndentityId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_U_PlatformUser_Deleted",
                table: "U_PlatformUser",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "IX_U_PlatformUser_ID",
                table: "U_PlatformUser",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_U_PlatformUser_UserId",
                table: "U_PlatformUser",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "U_PlatformUser");
        }
    }
}
