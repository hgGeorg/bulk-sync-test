using Microsoft.EntityFrameworkCore.Migrations;

namespace BulkSyncTest.Migrations
{
    public partial class ComplexTypes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Referenced",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Referenced", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProfileTemplates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OwnedA_ReferencedId = table.Column<int>(type: "int", nullable: true),
                    OwnedB_Content = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileTemplates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProfileTemplates_Referenced_OwnedA_ReferencedId",
                        column: x => x.OwnedA_ReferencedId,
                        principalTable: "Referenced",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProfileTemplates_OwnedA_ReferencedId",
                table: "ProfileTemplates",
                column: "OwnedA_ReferencedId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProfileTemplates");

            migrationBuilder.DropTable(
                name: "Referenced");
        }
    }
}
