using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Optsol.Playground.Infra.Migrations
{
    public partial class CriandoTabelaCartaCredito : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CartaoCredito",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    NomeCliente = table.Column<string>(maxLength: 200, nullable: false),
                    Numero = table.Column<string>(maxLength: 200, nullable: false),
                    CodigoVerificacao = table.Column<string>(maxLength: 200, nullable: false),
                    Validade = table.Column<string>(maxLength: 200, nullable: false),
                    ClienteId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartaoCredito", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartaoCredito_Cliente_ClienteId",
                        column: x => x.ClienteId,
                        principalTable: "Cliente",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartaoCredito_ClienteId",
                table: "CartaoCredito",
                column: "ClienteId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartaoCredito");
        }
    }
}
