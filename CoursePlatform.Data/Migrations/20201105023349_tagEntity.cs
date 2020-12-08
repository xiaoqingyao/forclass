using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoursePlatform.Data.Migrations
{
    public partial class tagEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "B_Course_Tags",
                columns: table => new
                {
                    IndentityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Deleted = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    CourseId = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    SchoolId = table.Column<int>(nullable: false),
                    RegtionId = table.Column<int>(nullable: false),
                    Creator = table.Column<int>(nullable: false),
                    AssetId = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    TypeName = table.Column<string>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_B_Course_Tags", x => x.IndentityId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_B_Course_Tags_Deleted",
                table: "B_Course_Tags",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "IX_B_Course_Tags_ID",
                table: "B_Course_Tags",
                column: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "B_Course_Tags");
        }
    }
}
