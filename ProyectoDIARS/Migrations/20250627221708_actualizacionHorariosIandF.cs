using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoDIARS.Migrations
{
    /// <inheritdoc />
    public partial class actualizacionHorariosIandF : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Horario",
                table: "Cursos",
                newName: "HorarioInicio");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "HorarioFin",
                table: "Cursos",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HorarioFin",
                table: "Cursos");

            migrationBuilder.RenameColumn(
                name: "HorarioInicio",
                table: "Cursos",
                newName: "Horario");
        }
    }
}
