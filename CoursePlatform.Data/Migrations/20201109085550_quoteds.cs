using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoursePlatform.Data.Migrations
{
    public partial class quoteds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "B_Course_DS",
                columns: table => new
                {
                    IndentityId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ID = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Deleted = table.Column<int>(nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: true),
                    UpdateTime = table.Column<DateTime>(nullable: true),
                    OperatorId = table.Column<int>(nullable: false),
                    DsId = table.Column<Guid>(nullable: false),
                    SortVal = table.Column<int>(nullable: false),
                    CatalogId = table.Column<int>(nullable: false),
                    IsOpen = table.Column<bool>(nullable: false),
                    CourseId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_B_Course_DS", x => x.IndentityId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_B_Course_DS_CatalogId",
                table: "B_Course_DS",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_B_Course_DS_CourseId",
                table: "B_Course_DS",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_B_Course_DS_Deleted",
                table: "B_Course_DS",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "IX_B_Course_DS_ID",
                table: "B_Course_DS",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_B_Course_DS_OperatorId",
                table: "B_Course_DS",
                column: "OperatorId");

            migrationBuilder.CreateIndex(
                name: "IX_B_Course_DS_SortVal",
                table: "B_Course_DS",
                column: "SortVal");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "B_Course_DS");
        }
    }
}
