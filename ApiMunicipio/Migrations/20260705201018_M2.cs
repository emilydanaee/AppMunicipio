using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiMunicipio.Migrations
{
    /// <inheritdoc />
    public partial class M2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitud",
                table: "SituacionCalle",
                type: "decimal(18,6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitud",
                table: "SituacionCalle",
                type: "decimal(18,6)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<string>(
                name: "Condicion",
                table: "SituacionCalle",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Direccion",
                table: "SituacionCalle",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Prioridad",
                table: "SituacionCalle",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "RiesgoInmediato",
                table: "SituacionCalle",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Sector",
                table: "SituacionCalle",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Condicion",
                table: "SituacionCalle");

            migrationBuilder.DropColumn(
                name: "Direccion",
                table: "SituacionCalle");

            migrationBuilder.DropColumn(
                name: "Prioridad",
                table: "SituacionCalle");

            migrationBuilder.DropColumn(
                name: "RiesgoInmediato",
                table: "SituacionCalle");

            migrationBuilder.DropColumn(
                name: "Sector",
                table: "SituacionCalle");

            migrationBuilder.AlterColumn<decimal>(
                name: "Longitud",
                table: "SituacionCalle",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitud",
                table: "SituacionCalle",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,6)");
        }
    }
}
