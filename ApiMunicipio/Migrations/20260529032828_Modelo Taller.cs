using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiMunicipio.Migrations
{
    /// <inheritdoc />
    public partial class ModeloTaller : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Talleres",
                columns: table => new
                {
                    IdTaller = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreTaller = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescripcionTaller = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaInicio = table.Column<DateOnly>(type: "date", nullable: false),
                    FechaFin = table.Column<DateOnly>(type: "date", nullable: false),
                    HoraInicio = table.Column<TimeOnly>(type: "time", nullable: false),
                    HoraFin = table.Column<TimeOnly>(type: "time", nullable: false),
                    SectorTaller = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CuposTaller = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Talleres", x => x.IdTaller);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Talleres");
        }
    }
}
