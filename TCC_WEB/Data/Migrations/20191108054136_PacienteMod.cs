using Microsoft.EntityFrameworkCore.Migrations;

namespace TCC_WEB.Data.Migrations
{
    public partial class PacienteMod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Pacientes",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Pacientes");
        }
    }
}
