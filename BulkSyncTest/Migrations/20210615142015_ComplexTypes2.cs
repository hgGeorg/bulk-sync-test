using Microsoft.EntityFrameworkCore.Migrations;

namespace BulkSyncTest.Migrations
{
    public partial class ComplexTypes2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileTemplates_Referenced_OwnedA_ReferencedId",
                table: "ProfileTemplates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProfileTemplates",
                table: "ProfileTemplates");

            migrationBuilder.RenameTable(
                name: "ProfileTemplates",
                newName: "ComplexTypes");

            migrationBuilder.RenameIndex(
                name: "IX_ProfileTemplates_OwnedA_ReferencedId",
                table: "ComplexTypes",
                newName: "IX_ComplexTypes_OwnedA_ReferencedId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ComplexTypes",
                table: "ComplexTypes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ComplexTypes_Referenced_OwnedA_ReferencedId",
                table: "ComplexTypes",
                column: "OwnedA_ReferencedId",
                principalTable: "Referenced",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComplexTypes_Referenced_OwnedA_ReferencedId",
                table: "ComplexTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ComplexTypes",
                table: "ComplexTypes");

            migrationBuilder.RenameTable(
                name: "ComplexTypes",
                newName: "ProfileTemplates");

            migrationBuilder.RenameIndex(
                name: "IX_ComplexTypes_OwnedA_ReferencedId",
                table: "ProfileTemplates",
                newName: "IX_ProfileTemplates_OwnedA_ReferencedId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProfileTemplates",
                table: "ProfileTemplates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileTemplates_Referenced_OwnedA_ReferencedId",
                table: "ProfileTemplates",
                column: "OwnedA_ReferencedId",
                principalTable: "Referenced",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
