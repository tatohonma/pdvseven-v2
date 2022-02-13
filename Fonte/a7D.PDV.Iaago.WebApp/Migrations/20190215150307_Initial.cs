using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace a7D.PDV.Iaago.WebApp.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Perguntas",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Texto = table.Column<string>(maxLength: 1000, nullable: false),
                    Luis = table.Column<string>(maxLength: 50, nullable: true),
                    Intencao = table.Column<string>(maxLength: 50, nullable: true),
                    Score = table.Column<double>(nullable: false),
                    Entidade1 = table.Column<string>(maxLength: 50, nullable: true),
                    Valor1 = table.Column<string>(maxLength: 100, nullable: true),
                    Entidade2 = table.Column<string>(maxLength: 50, nullable: true),
                    Valor2 = table.Column<string>(maxLength: 100, nullable: true),
                    Entidade3 = table.Column<string>(maxLength: 50, nullable: true),
                    Valor3 = table.Column<string>(maxLength: 100, nullable: true),
                    QTD = table.Column<double>(nullable: false),
                    Criacao = table.Column<DateTime>(nullable: false),
                    UltimaAtualizacao = table.Column<DateTime>(nullable: false),
                    UltimoUso = table.Column<DateTime>(nullable: false),
                    Retorno = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Perguntas", x => x.ID);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Perguntas");
        }
    }
}
