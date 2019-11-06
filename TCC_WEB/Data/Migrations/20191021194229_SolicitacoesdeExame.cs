using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TCC_WEB.Data.Migrations
{
    public partial class SolicitacoesdeExame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Médico",
                table: "Receitas",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Dose",
                table: "Receitas",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "SolicitacoesdeExame",
                columns: table => new
                {
                    SolicitacaodeExameId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Data = table.Column<DateTime>(nullable: false),
                    TipodeExameId = table.Column<int>(nullable: false),
                    Médico = table.Column<string>(maxLength: 150, nullable: false),
                    PacienteId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SolicitacoesdeExame", x => x.SolicitacaodeExameId);
                    table.ForeignKey(
                        name: "FK_SolicitacoesdeExame_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "PacienteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SolicitacoesdeExame_TipodeExame_TipodeExameId",
                        column: x => x.TipodeExameId,
                        principalTable: "TipodeExame",
                        principalColumn: "TipodeExameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacoesdeExame_PacienteId",
                table: "SolicitacoesdeExame",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_SolicitacoesdeExame_TipodeExameId",
                table: "SolicitacoesdeExame",
                column: "TipodeExameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SolicitacoesdeExame");

            migrationBuilder.AlterColumn<string>(
                name: "Médico",
                table: "Receitas",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "Dose",
                table: "Receitas",
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100);
        }
    }
}
