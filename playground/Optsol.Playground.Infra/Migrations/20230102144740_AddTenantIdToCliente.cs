using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Optsol.Playground.Infra.Migrations
{
    public partial class AddTenantIdToCliente : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "Cliente",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "TenantId",
                table: "CartaoCredito",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Cliente");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "CartaoCredito");
        }
    }
}
