using Microsoft.EntityFrameworkCore.Migrations;

namespace Optsol.Playground.Infra.Migrations
{
    public partial class AlerandoCampoCreateDateParaCreatedDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "Cliente",
                newName: "CreatedDate");

            migrationBuilder.RenameColumn(
                name: "CreateDate",
                table: "CartaoCredito",
                newName: "CreatedDate");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Cliente",
                newName: "CreateDate");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "CartaoCredito",
                newName: "CreateDate");
        }
    }
}
