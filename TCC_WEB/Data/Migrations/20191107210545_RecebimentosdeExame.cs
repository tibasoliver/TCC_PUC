using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TCC_WEB.Data.Migrations
{
    public partial class RecebimentosdeExame : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecebimentosdeExame",
                columns: table => new
                {
                    RecebimentodeExameId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Data = table.Column<DateTime>(nullable: false),
                    TipodeExameId = table.Column<int>(nullable: false),
                    Dado = table.Column<string>(nullable: false),
                    PacienteId = table.Column<int>(nullable: false),
                    recebe = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecebimentosdeExame", x => x.RecebimentodeExameId);
                    table.ForeignKey(
                        name: "FK_RecebimentosdeExame_Pacientes_PacienteId",
                        column: x => x.PacienteId,
                        principalTable: "Pacientes",
                        principalColumn: "PacienteId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecebimentosdeExame_TipodeExame_TipodeExameId",
                        column: x => x.TipodeExameId,
                        principalTable: "TipodeExame",
                        principalColumn: "TipodeExameId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecebimentosdeExame_PacienteId",
                table: "RecebimentosdeExame",
                column: "PacienteId");

            migrationBuilder.CreateIndex(
                name: "IX_RecebimentosdeExame_TipodeExameId",
                table: "RecebimentosdeExame",
                column: "TipodeExameId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecebimentosdeExame");
        }
    }
}
