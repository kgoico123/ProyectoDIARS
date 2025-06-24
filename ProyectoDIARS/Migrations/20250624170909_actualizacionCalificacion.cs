using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoDIARS.Migrations
{
    /// <inheritdoc />
    public partial class actualizacionCalificacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Estudiante_Cursos_CursoId",
                table: "Estudiante_Cursos");

            migrationBuilder.AddColumn<string>(
                name: "Comentario",
                table: "Calificacion",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Estudiante_Cursos_CursoId",
                table: "Estudiante_Cursos",
                column: "CursoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Estudiante_Cursos_CursoId",
                table: "Estudiante_Cursos");

            migrationBuilder.DropColumn(
                name: "Comentario",
                table: "Calificacion");

            migrationBuilder.CreateIndex(
                name: "IX_Estudiante_Cursos_CursoId",
                table: "Estudiante_Cursos",
                column: "CursoId",
                unique: true);
        }
    }
}
