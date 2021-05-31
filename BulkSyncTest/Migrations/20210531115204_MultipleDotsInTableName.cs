using Microsoft.EntityFrameworkCore.Migrations;

namespace BulkSyncTest.Migrations
{
    public partial class MultipleDotsInTableName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Table",
                table: "Table");

            migrationBuilder.RenameTable(
                name: "Table",
                newName: "Inheritance");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Inheritance",
                table: "Inheritance",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Entity.With.Multiple.Dots.In.Table.Name",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Entity.With.Multiple.Dots.In.Table.Name", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Entity.With.Multiple.Dots.In.Table.Name");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Inheritance",
                table: "Inheritance");

            migrationBuilder.RenameTable(
                name: "Inheritance",
                newName: "Table");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Table",
                table: "Table",
                column: "Id");
        }
    }
}
