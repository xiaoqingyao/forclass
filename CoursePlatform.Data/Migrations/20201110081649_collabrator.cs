using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoursePlatform.Data.Migrations
{
    public partial class collabrator : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "B_Course_Collabrator",
                columns: table => new
                {
                    IndentityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Deleted = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    CouserId = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CollabratorId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_B_Course_Collabrator", x => x.IndentityId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_B_Course_Collabrator_CollabratorId",
                table: "B_Course_Collabrator",
                column: "CollabratorId");

            migrationBuilder.CreateIndex(
                name: "IX_B_Course_Collabrator_CouserId",
                table: "B_Course_Collabrator",
                column: "CouserId");

            migrationBuilder.CreateIndex(
                name: "IX_B_Course_Collabrator_Deleted",
                table: "B_Course_Collabrator",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "IX_B_Course_Collabrator_ID",
                table: "B_Course_Collabrator",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "B_Course_Collabrator");
        }
    }
}
