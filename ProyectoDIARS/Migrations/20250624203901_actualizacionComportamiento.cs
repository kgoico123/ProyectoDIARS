using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProyectoDIARS.Migrations
{
    /// <inheritdoc />
    public partial class actualizacionComportamiento : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Calificacion",
                table: "Comportamientos",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Calificacion",
                table: "Comportamientos");
        }
    }
}
