using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ApiMunicipio.Migrations
{
    /// <inheritdoc />
    public partial class AgregarInstructorDiasModalidad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Talleres",
                keyColumn: "IdTaller",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Talleres",
                keyColumn: "IdTaller",
                keyValue: 2);

            migrationBuilder.AddColumn<string>(
                name: "Dias",
                table: "Talleres",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Instructor",
                table: "Talleres",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Modalidad",
                table: "Talleres",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Dias",
                table: "Talleres");

            migrationBuilder.DropColumn(
                name: "Instructor",
                table: "Talleres");

            migrationBuilder.DropColumn(
                name: "Modalidad",
                table: "Talleres");

            migrationBuilder.InsertData(
                table: "Talleres",
                columns: new[] { "IdTaller", "CuposTaller", "DescripcionTaller", "FechaFin", "FechaInicio", "HoraFin", "HoraInicio", "ImagenTaller", "Inscritos", "NombreTaller", "SectorTaller" },
                values: new object[,]
                {
                    { 1, 40, "Capacitación comunitaria", new DateOnly(2026, 7, 30), new DateOnly(2026, 6, 1), new TimeOnly(20, 0, 0), new TimeOnly(18, 0, 0), null, 0, "Taller de Cocina", "Quitumbe" },
                    { 2, 40, "Capacitación comunitaria", new DateOnly(2026, 12, 30), new DateOnly(2026, 7, 1), new TimeOnly(12, 0, 0), new TimeOnly(10, 0, 0), null, 0, "Taller de Baile", "Solanda" }
                });
        }
    }
}
