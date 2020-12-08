using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CoursePlatform.Data.Migrations
{
    public partial class AuditLog2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "B_Course_Audit_Log",
                columns: table => new
                {
                    IndentityId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CouserId = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    ReviewerName = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    ReviewerId = table.Column<int>(type: "int", nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    ReviewerOrgId = table.Column<int>(type: "int", nullable: false),
                    ReviewerOrgName = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    StatusDesc = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    ID = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Deleted = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdateTime = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_B_Course_Audit_Log", x => x.IndentityId);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "B_Course_Audit_Log");
        }
    }
}
