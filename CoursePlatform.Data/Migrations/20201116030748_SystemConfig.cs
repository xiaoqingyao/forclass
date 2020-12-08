using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoursePlatform.Data.Migrations
{
    public partial class SystemConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_U_platformUser_Course_CourseId",
                table: "U_platformUser_Course");

            migrationBuilder.DropIndex(
                name: "IX_U_platformUser_Course_Deleted",
                table: "U_platformUser_Course");

            migrationBuilder.DropIndex(
                name: "IX_U_platformUser_Course_ID",
                table: "U_platformUser_Course");

            migrationBuilder.DropIndex(
                name: "IX_U_platformUser_Course_PlatUserId",
                table: "U_platformUser_Course");

            migrationBuilder.DropIndex(
                name: "IX_U_platformUser_Course_UserId",
                table: "U_platformUser_Course");

            migrationBuilder.DropIndex(
                name: "IX_U_PlatformUser_Deleted",
                table: "U_PlatformUser");

            migrationBuilder.DropIndex(
                name: "IX_U_PlatformUser_ID",
                table: "U_PlatformUser");

            migrationBuilder.DropIndex(
                name: "IX_U_PlatformUser_UserId",
                table: "U_PlatformUser");

            migrationBuilder.DropIndex(
                name: "IX_B_Course_Tags_Deleted",
                table: "B_Course_Tags");

            migrationBuilder.DropIndex(
                name: "IX_B_Course_Tags_ID",
                table: "B_Course_Tags");

            migrationBuilder.DropIndex(
                name: "IX_B_Course_DS_CatalogId",
                table: "B_Course_DS");

            migrationBuilder.DropIndex(
                name: "IX_B_Course_DS_CourseId",
                table: "B_Course_DS");

            migrationBuilder.DropIndex(
                name: "IX_B_Course_DS_Deleted",
                table: "B_Course_DS");

            migrationBuilder.DropIndex(
                name: "IX_B_Course_DS_ID",
                table: "B_Course_DS");

            migrationBuilder.DropIndex(
                name: "IX_B_Course_DS_OperatorId",
                table: "B_Course_DS");

            migrationBuilder.DropIndex(
                name: "IX_B_Course_DS_SortVal",
                table: "B_Course_DS");

            migrationBuilder.DropIndex(
                name: "IX_B_Course_Collabrator_CollabratorId",
                table: "B_Course_Collabrator");

            migrationBuilder.DropIndex(
                name: "IX_B_Course_Collabrator_CouserId",
                table: "B_Course_Collabrator");

            migrationBuilder.DropIndex(
                name: "IX_B_Course_Collabrator_Deleted",
                table: "B_Course_Collabrator");

            migrationBuilder.DropIndex(
                name: "IX_B_Course_Collabrator_ID",
                table: "B_Course_Collabrator");

            migrationBuilder.DropIndex(
                name: "IX_B_Course_CollbratorCount",
                table: "B_Course");

            migrationBuilder.DropIndex(
                name: "IX_B_Course_CreatorCode",
                table: "B_Course");

            migrationBuilder.DropIndex(
                name: "IX_B_Course_Deleted",
                table: "B_Course");

            migrationBuilder.DropIndex(
                name: "IX_B_Course_ID",
                table: "B_Course");

            migrationBuilder.CreateTable(
                name: "B_SystemConfig",
                columns: table => new
                {
                    IndentityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagAttr = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    ID = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Deleted = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_B_SystemConfig", x => x.IndentityId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "B_SystemConfig");

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

            migrationBuilder.CreateIndex(
                name: "IX_B_Course_Tags_Deleted",
                table: "B_Course_Tags",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "IX_B_Course_Tags_ID",
                table: "B_Course_Tags",
                column: "ID");

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

            migrationBuilder.CreateIndex(
                name: "IX_B_Course_CollbratorCount",
                table: "B_Course",
                column: "CollbratorCount");

            migrationBuilder.CreateIndex(
                name: "IX_B_Course_CreatorCode",
                table: "B_Course",
                column: "CreatorCode");

            migrationBuilder.CreateIndex(
                name: "IX_B_Course_Deleted",
                table: "B_Course",
                column: "Deleted");

            migrationBuilder.CreateIndex(
                name: "IX_B_Course_ID",
                table: "B_Course",
                column: "ID");
        }
    }
}
