using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiMunicipio.Migrations
{
    /// <inheritdoc />
    public partial class modeloscamb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NombreReportado",
                table: "SituacionCalle");

            migrationBuilder.DropColumn(
                name: "TallerId",
                table: "Inscripciones");

            migrationBuilder.RenameColumn(
                name: "Ubicacion",
                table: "SituacionCalle",
                newName: "NombreReportante");

            migrationBuilder.RenameColumn(
                name: "UbicacionReporte",
                table: "Reportes",
                newName: "Telefono");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Permisos",
                newName: "Observaciones");

            migrationBuilder.RenameColumn(
                name: "UsuarioId",
                table: "Inscripciones",
                newName: "Telefono");

            migrationBuilder.RenameColumn(
                name: "TituloAlerta",
                table: "Alertas",
                newName: "TipoAlerta");

            migrationBuilder.AddColumn<string>(
                name: "ImagenTaller",
                table: "Talleres",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaReporte",
                table: "SituacionCalle",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "Latitud",
                table: "SituacionCalle",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitud",
                table: "SituacionCalle",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "DocumentoAdjunto",
                table: "Reservas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "MotivoReserva",
                table: "Reservas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Observaciones",
                table: "Reservas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cedula",
                table: "ReportesViolencia",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Correo",
                table: "ReportesViolencia",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Latitud",
                table: "ReportesViolencia",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitud",
                table: "ReportesViolencia",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Referencia",
                table: "ReportesViolencia",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "ReportesViolencia",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cedula",
                table: "Reportes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Correo",
                table: "Reportes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImagenReporte",
                table: "Reportes",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Latitud",
                table: "Reportes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitud",
                table: "Reportes",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "DocumentoAdjunto",
                table: "Permisos",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cedula",
                table: "Inscripciones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Correo",
                table: "Inscripciones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DescripcionActividad",
                table: "Inscripciones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreActividad",
                table: "Inscripciones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "NombreCompleto",
                table: "Inscripciones",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "DireccionFeria",
                table: "Ferias",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "HoraFin",
                table: "Ferias",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<TimeOnly>(
                name: "HoraInicio",
                table: "Ferias",
                type: "time",
                nullable: false,
                defaultValue: new TimeOnly(0, 0, 0));

            migrationBuilder.AddColumn<string>(
                name: "ImagenFeria",
                table: "Ferias",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Pagado",
                table: "Consultas",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "ImagenCampania",
                table: "Campanias",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Cedula",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaRegistro",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Sector",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Cedula",
                table: "Alertas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Correo",
                table: "Alertas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Latitud",
                table: "Alertas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "Longitud",
                table: "Alertas",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Sector",
                table: "Alertas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Telefono",
                table: "Alertas",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Obras",
                columns: table => new
                {
                    IdObra = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NombreObra = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DescripcionObra = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SectorObra = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitud = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Longitud = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FechaSolicitud = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EstadoObra = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Obras", x => x.IdObra);
                });

            migrationBuilder.UpdateData(
                table: "Talleres",
                keyColumn: "IdTaller",
                keyValue: 1,
                column: "ImagenTaller",
                value: null);

            migrationBuilder.UpdateData(
                table: "Talleres",
                keyColumn: "IdTaller",
                keyValue: 2,
                column: "ImagenTaller",
                value: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Obras");

            migrationBuilder.DropColumn(
                name: "ImagenTaller",
                table: "Talleres");

            migrationBuilder.DropColumn(
                name: "FechaReporte",
                table: "SituacionCalle");

            migrationBuilder.DropColumn(
                name: "Latitud",
                table: "SituacionCalle");

            migrationBuilder.DropColumn(
                name: "Longitud",
                table: "SituacionCalle");

            migrationBuilder.DropColumn(
                name: "DocumentoAdjunto",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "MotivoReserva",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "Observaciones",
                table: "Reservas");

            migrationBuilder.DropColumn(
                name: "Cedula",
                table: "ReportesViolencia");

            migrationBuilder.DropColumn(
                name: "Correo",
                table: "ReportesViolencia");

            migrationBuilder.DropColumn(
                name: "Latitud",
                table: "ReportesViolencia");

            migrationBuilder.DropColumn(
                name: "Longitud",
                table: "ReportesViolencia");

            migrationBuilder.DropColumn(
                name: "Referencia",
                table: "ReportesViolencia");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "ReportesViolencia");

            migrationBuilder.DropColumn(
                name: "Cedula",
                table: "Reportes");

            migrationBuilder.DropColumn(
                name: "Correo",
                table: "Reportes");

            migrationBuilder.DropColumn(
                name: "ImagenReporte",
                table: "Reportes");

            migrationBuilder.DropColumn(
                name: "Latitud",
                table: "Reportes");

            migrationBuilder.DropColumn(
                name: "Longitud",
                table: "Reportes");

            migrationBuilder.DropColumn(
                name: "DocumentoAdjunto",
                table: "Permisos");

            migrationBuilder.DropColumn(
                name: "Cedula",
                table: "Inscripciones");

            migrationBuilder.DropColumn(
                name: "Correo",
                table: "Inscripciones");

            migrationBuilder.DropColumn(
                name: "DescripcionActividad",
                table: "Inscripciones");

            migrationBuilder.DropColumn(
                name: "NombreActividad",
                table: "Inscripciones");

            migrationBuilder.DropColumn(
                name: "NombreCompleto",
                table: "Inscripciones");

            migrationBuilder.DropColumn(
                name: "DireccionFeria",
                table: "Ferias");

            migrationBuilder.DropColumn(
                name: "HoraFin",
                table: "Ferias");

            migrationBuilder.DropColumn(
                name: "HoraInicio",
                table: "Ferias");

            migrationBuilder.DropColumn(
                name: "ImagenFeria",
                table: "Ferias");

            migrationBuilder.DropColumn(
                name: "Pagado",
                table: "Consultas");

            migrationBuilder.DropColumn(
                name: "ImagenCampania",
                table: "Campanias");

            migrationBuilder.DropColumn(
                name: "Cedula",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "FechaRegistro",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Sector",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Cedula",
                table: "Alertas");

            migrationBuilder.DropColumn(
                name: "Correo",
                table: "Alertas");

            migrationBuilder.DropColumn(
                name: "Latitud",
                table: "Alertas");

            migrationBuilder.DropColumn(
                name: "Longitud",
                table: "Alertas");

            migrationBuilder.DropColumn(
                name: "Sector",
                table: "Alertas");

            migrationBuilder.DropColumn(
                name: "Telefono",
                table: "Alertas");

            migrationBuilder.RenameColumn(
                name: "NombreReportante",
                table: "SituacionCalle",
                newName: "Ubicacion");

            migrationBuilder.RenameColumn(
                name: "Telefono",
                table: "Reportes",
                newName: "UbicacionReporte");

            migrationBuilder.RenameColumn(
                name: "Observaciones",
                table: "Permisos",
                newName: "UsuarioId");

            migrationBuilder.RenameColumn(
                name: "Telefono",
                table: "Inscripciones",
                newName: "UsuarioId");

            migrationBuilder.RenameColumn(
                name: "TipoAlerta",
                table: "Alertas",
                newName: "TituloAlerta");

            migrationBuilder.AddColumn<string>(
                name: "NombreReportado",
                table: "SituacionCalle",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "TallerId",
                table: "Inscripciones",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
