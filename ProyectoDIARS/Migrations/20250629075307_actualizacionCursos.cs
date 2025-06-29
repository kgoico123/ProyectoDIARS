using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoDIARS.Migrations
{
    /// <inheritdoc />
    public partial class actualizacionCursos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Docentes_AspNetUsers_UserId",
                table: "Docentes");

            migrationBuilder.DropForeignKey(
                name: "FK_Docentes_Cursos_CursoIdCurso",
                table: "Docentes");

            migrationBuilder.RenameColumn(
                name: "CursoIdCurso",
                table: "Docentes",
                newName: "CursoId");

            migrationBuilder.RenameIndex(
                name: "IX_Docentes_CursoIdCurso",
                table: "Docentes",
                newName: "IX_Docentes_CursoId");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Docentes",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Docentes_AspNetUsers_UserId",
                table: "Docentes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Docentes_Cursos_CursoId",
                table: "Docentes",
                column: "CursoId",
                principalTable: "Cursos",
                principalColumn: "IdCurso",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Docentes_AspNetUsers_UserId",
                table: "Docentes");

            migrationBuilder.DropForeignKey(
                name: "FK_Docentes_Cursos_CursoId",
                table: "Docentes");

            migrationBuilder.RenameColumn(
                name: "CursoId",
                table: "Docentes",
                newName: "CursoIdCurso");

            migrationBuilder.RenameIndex(
                name: "IX_Docentes_CursoId",
                table: "Docentes",
                newName: "IX_Docentes_CursoIdCurso");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Docentes",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Docentes_AspNetUsers_UserId",
                table: "Docentes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Docentes_Cursos_CursoIdCurso",
                table: "Docentes",
                column: "CursoIdCurso",
                principalTable: "Cursos",
                principalColumn: "IdCurso");
        }
    }
}
