using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoDIARS.Migrations
{
    /// <inheritdoc />
    public partial class actualizacionCursoGrado : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Grado",
                table: "Cursos",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Grado",
                table: "Cursos");
        }
    }
}
