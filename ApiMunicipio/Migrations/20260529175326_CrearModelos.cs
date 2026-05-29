using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiMunicipio.Migrations
{
    /// <inheritdoc />
    public partial class CrearModelos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ferias",
                columns: table => new
                {
                    IdFeria = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreFeria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescripcionFeria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaFeria = table.Column<DateOnly>(type: "date", nullable: false),
                    SectorFeria = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumeroEmprendedores = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ferias", x => x.IdFeria);
                });

            migrationBuilder.CreateTable(
                name: "Reportes",
                columns: table => new
                {
                    IdReporte = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoReporte = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescripcionReporte = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaReporte = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UbicacionReporte = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EstadoReporte = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reportes", x => x.IdReporte);
                });

            migrationBuilder.CreateTable(
                name: "Solicitudes",
                columns: table => new
                {
                    IdSolicitud = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoSolicitud = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescripcionSolicitud = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FechaSolicitud = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstadoSolicitud = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Solicitudes", x => x.IdSolicitud);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Ferias");

            migrationBuilder.DropTable(
                name: "Reportes");

            migrationBuilder.DropTable(
                name: "Solicitudes");
        }
    }
}
