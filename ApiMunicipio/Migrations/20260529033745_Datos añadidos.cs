using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ApiMunicipio.Migrations
{
    /// <inheritdoc />
    public partial class Datosañadidos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Talleres",
                columns: new[] { "IdTaller", "CuposTaller", "DescripcionTaller", "FechaFin", "FechaInicio", "HoraFin", "HoraInicio", "NombreTaller", "SectorTaller" },
                values: new object[,]
                {
                    { 1, 40, "Capacitación comunitaria", new DateOnly(2026, 7, 30), new DateOnly(2026, 6, 1), new TimeOnly(20, 0, 0), new TimeOnly(18, 0, 0), "Taller de Cocina", "Quitumbe" },
                    { 2, 40, "Capacitación comunitaria", new DateOnly(2026, 12, 30), new DateOnly(2026, 7, 1), new TimeOnly(12, 0, 0), new TimeOnly(10, 0, 0), "Taller de Baile", "Solanda" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Talleres",
                keyColumn: "IdTaller",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Talleres",
                keyColumn: "IdTaller",
                keyValue: 2);
        }
    }
}
