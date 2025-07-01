using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoDIARS.Migrations
{
    /// <inheritdoc />
    public partial class actualizacionModelo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocenteId",
                table: "Cursos");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DocenteId",
                table: "Cursos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
