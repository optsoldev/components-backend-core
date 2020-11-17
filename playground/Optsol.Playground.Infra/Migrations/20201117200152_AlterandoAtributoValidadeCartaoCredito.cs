using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Optsol.Playground.Infra.Migrations
{
    public partial class AlterandoAtributoValidadeCartaoCredito : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Validade",
                table: "CartaoCredito",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Validade",
                table: "CartaoCredito",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(DateTime),
                oldMaxLength: 200);
        }
    }
}
