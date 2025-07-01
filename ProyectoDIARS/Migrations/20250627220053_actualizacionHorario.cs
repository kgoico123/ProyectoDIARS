using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoDIARS.Migrations
{
    /// <inheritdoc />
    public partial class actualizacionHorario : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "horario",
                table: "Cursos",
                newName: "Horario");

            migrationBuilder.AlterColumn<TimeSpan>(
                name: "Horario",
                table: "Cursos",
                type: "time",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Horario",
                table: "Cursos",
                newName: "horario");

            migrationBuilder.AlterColumn<DateTime>(
                name: "horario",
                table: "Cursos",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(TimeSpan),
                oldType: "time");
        }
    }
}
