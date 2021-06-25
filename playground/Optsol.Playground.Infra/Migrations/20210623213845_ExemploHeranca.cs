using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Optsol.Playground.Infra.Migrations
{
    public partial class ExemploHeranca : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Cliente",
                newName: "ClienteId");

            migrationBuilder.CreateTable(
                name: "ClientePessoaFisica",
                columns: table => new
                {
                    ClienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Documento = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientePessoaFisica", x => x.ClienteId);
                    table.ForeignKey(
                        name: "FK_ClientePessoaFisica_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "ClienteId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ClientePessoaJuridica",
                columns: table => new
                {
                    ClienteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NumeroCnpj = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientePessoaJuridica", x => x.ClienteId);
                    table.ForeignKey(
                        name: "FK_ClientePessoaJuridica_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "ClienteId",
                        onDelete: ReferentialAction.Restrict);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientePessoaFisica");

            migrationBuilder.DropTable(
                name: "ClientePessoaJuridica");

            migrationBuilder.RenameColumn(
                name: "ClienteId",
                table: "Cliente",
                newName: "Id");
        }
    }
}
