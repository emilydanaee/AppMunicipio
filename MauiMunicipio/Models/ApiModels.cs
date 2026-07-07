using System.Text.Json.Serialization;

namespace MauiMunicipio.Models;

public sealed class LoginRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public sealed class RegisterRequest
{
    public string NombreCompleto { get; set; } = string.Empty;
    public string Cedula { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;
}

public sealed class LoginResponse
{
    public string? Id { get; set; }
    public string? Nombre { get; set; }
    public string? NombreCompleto { get; set; }
    public string? Email { get; set; }
    public string? Cedula { get; set; }
    public string? Sector { get; set; }
    public List<string>? Roles { get; set; }
}

public sealed class UserSession
{
    public string Id { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cedula { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = [];

    [JsonIgnore]
    public bool IsAdmin => Roles.Any(role => string.Equals(role, "Admin", StringComparison.OrdinalIgnoreCase));
}

public sealed class HealthResponse
{
    public string Status { get; set; } = string.Empty;
    public string Database { get; set; } = string.Empty;
    public DateTime Utc { get; set; }
}

public sealed class Taller
{
    public int IdTaller { get; set; }
    public string NombreTaller { get; set; } = string.Empty;
    public string DescripcionTaller { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string? HoraInicio { get; set; }
    public string? HoraFin { get; set; }
    public string SectorTaller { get; set; } = string.Empty;
    public string UbicacionTaller { get; set; } = string.Empty;
    public string Instructor { get; set; } = string.Empty;
    public string Dias { get; set; } = string.Empty;
    public string Modalidad { get; set; } = string.Empty;
    public string? ImagenTaller { get; set; }
    public int CuposTaller { get; set; }
    public int Inscritos { get; set; }
    public int CuposDisponibles { get; set; }
}

public sealed class Feria
{
    public int IdFeria { get; set; }
    public string NombreFeria { get; set; } = string.Empty;
    public string DescripcionFeria { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string? HoraInicio { get; set; }
    public string? HoraFin { get; set; }
    public string SectorFeria { get; set; } = string.Empty;
    public string UbicacionFeria { get; set; } = string.Empty;
    public string? ImagenFeria { get; set; }
}

public sealed class Campania
{
    public int IdCampania { get; set; }
    public string NombreCampania { get; set; } = string.Empty;
    public string TipoCampania { get; set; } = string.Empty;
    public string DescripcionCampania { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string? HoraInicio { get; set; }
    public string? HoraFin { get; set; }
    public string UbicacionCampania { get; set; } = string.Empty;
    public string ResponsableCampania { get; set; } = string.Empty;
    public string? ImagenCampania { get; set; }
}

public sealed class TallerInscripcionRequest
{
    public int IdTaller { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Cedula { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
}

public sealed class WorkshopRegistration
{
    public int IdInscripcion { get; set; }
    public int IdTaller { get; set; }
    public string NombreTaller { get; set; } = string.Empty;
    public string DescripcionTaller { get; set; } = string.Empty;
    public DateTime FechaInicio { get; set; }
    public string HoraInicio { get; set; } = string.Empty;
    public string HoraFin { get; set; } = string.Empty;
    public string UbicacionTaller { get; set; } = string.Empty;
    public string SectorTaller { get; set; } = string.Empty;
    public string Modalidad { get; set; } = string.Empty;
    public DateTime FechaInscripcion { get; set; }
    public string Telefono { get; set; } = string.Empty;

    [JsonIgnore]
    public string DateText => FechaInicio == default ? "Fecha por confirmar" : FechaInicio.ToString("dd MMM yyyy");

    [JsonIgnore]
    public string TimeText => $"{ShortTime(HoraInicio)} - {ShortTime(HoraFin)}";

    [JsonIgnore]
    public string LocationText => string.IsNullOrWhiteSpace(UbicacionTaller) ? SectorTaller : UbicacionTaller;

    private static string ShortTime(string? value) =>
        string.IsNullOrWhiteSpace(value) ? "--:--" : value.Length >= 5 ? value[..5] : value;
}

public sealed class Reporte
{
    public int IdReporte { get; set; }
    public string TipoReporte { get; set; } = string.Empty;
    public string DescripcionReporte { get; set; } = string.Empty;
    public string Parroquia { get; set; } = string.Empty;
    public string AdministracionZonal { get; set; } = string.Empty;
    public DateTime FechaReporte { get; set; }
    public double Latitud { get; set; }
    public double Longitud { get; set; }
    public string Direccion { get; set; } = string.Empty;
    public string EstadoReporte { get; set; } = string.Empty;
    public string Cedula { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string? ImagenReporte { get; set; }
}

public sealed class ReportCard
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DateText { get; set; } = string.Empty;
    public string LocationText { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string StatusColor { get; set; } = "#0878D1";
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool HasLocation => Math.Abs(Latitude) > 0.000001 || Math.Abs(Longitude) > 0.000001;
}

public sealed class Alerta
{
    public int IdAlerta { get; set; }
    public string TipoAlerta { get; set; } = string.Empty;
    public string DescripcionAlerta { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
    public string Sector { get; set; } = string.Empty;
    public double Latitud { get; set; }
    public double Longitud { get; set; }
    public string Direccion { get; set; } = string.Empty;
    public string EstadoAlerta { get; set; } = string.Empty;
    public string Cedula { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string? ImagenAlerta { get; set; }
}

public sealed class Permiso
{
    public int IdPermiso { get; set; }
    public string TipoPermiso { get; set; } = string.Empty;
    public string DescripcionPermiso { get; set; } = string.Empty;
    public DateTime FechaPermiso { get; set; }
    public string EstadoPermiso { get; set; } = string.Empty;
    public string DocumentoAdjunto { get; set; } = string.Empty;
    public string Observaciones { get; set; } = string.Empty;
}

public sealed class Reserva
{
    public int IdReserva { get; set; }
    public string EspacioReserva { get; set; } = string.Empty;
    public string FechaReserva { get; set; } = string.Empty;
    public string HoraInicio { get; set; } = string.Empty;
    public string HoraFin { get; set; } = string.Empty;
    public string MotivoReserva { get; set; } = string.Empty;
    public string EstadoReserva { get; set; } = string.Empty;
    public string Observaciones { get; set; } = string.Empty;
    public string DocumentoAdjunto { get; set; } = string.Empty;
}

public sealed class Consulta
{
    public int IdConsulta { get; set; }
    public string Cedula { get; set; } = string.Empty;
    public decimal ValorPendiente { get; set; }
    public DateTime FechaConsulta { get; set; }
    public bool Pagado { get; set; }
}

public sealed class ActivityCard
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Badge { get; set; } = string.Empty;
    public string BadgeColor { get; set; } = "#0878D1";
    public string AccentColor { get; set; } = "#0878D1";
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DateText { get; set; } = string.Empty;
    public string TimeText { get; set; } = string.Empty;
    public string LocationText { get; set; } = string.Empty;
    public string CapacityText { get; set; } = string.Empty;
    public string ButtonText { get; set; } = string.Empty;
    public string? ImageUrl { get; set; }
}

public sealed class RequestCard
{
    public int Id { get; set; }
    public string Icon { get; set; } = "📄";
    public string Type { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DateText { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string StatusColor { get; set; } = "#0878D1";
    public bool CanCancel { get; set; }
}

public sealed class InscripcionGeneral
{
    public int IdInscripcion { get; set; }
    public string NombreActividad { get; set; } = string.Empty;
    public string DescripcionActividad { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string Cedula { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public DateTime FechaInscripcion { get; set; }
    public string Estado { get; set; } = string.Empty;
}

public sealed class SituacionCalle
{
    public int IdSituacionCalle { get; set; }
    public string NombreReportante { get; set; } = string.Empty;
    public DateTime FechaReporte { get; set; }
    public string Sector { get; set; } = string.Empty;
    public double Latitud { get; set; }
    public double Longitud { get; set; }
    public string Direccion { get; set; } = string.Empty;
    public string Condicion { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Prioridad { get; set; } = string.Empty;
    public bool RiesgoInmediato { get; set; }
    public string Estado { get; set; } = string.Empty;
}

public sealed class ReporteViolencia
{
    public int IdReporteViolencia { get; set; }
    public string TipoViolencia { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public double Latitud { get; set; }
    public double Longitud { get; set; }
    public string Referencia { get; set; } = string.Empty;
    public string Cedula { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Estado { get; set; } = string.Empty;
    public DateTime FechaReporte { get; set; }
}

public sealed class Obra
{
    public int IdObra { get; set; }
    public string NombreObra { get; set; } = string.Empty;
    public string DescripcionObra { get; set; } = string.Empty;
    public string SectorObra { get; set; } = string.Empty;
    public double Latitud { get; set; }
    public double Longitud { get; set; }
    public DateTime FechaSolicitud { get; set; }
    public string EstadoObra { get; set; } = string.Empty;
}

public sealed class WorkshopAttendee
{
    public int Id { get; set; }
    public int IdTaller { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Cedula { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public DateTime Fecha { get; set; }
}

public sealed class DocumentUploadResponse
{
    public string Path { get; set; } = string.Empty;
    public string FileName { get; set; } = string.Empty;
}

public sealed class ManagementCard
{
    public int Id { get; set; }
    public string Module { get; set; } = string.Empty;
    public string Icon { get; set; } = "▤";
    public string Title { get; set; } = string.Empty;
    public string Subtitle { get; set; } = string.Empty;
    public string Detail { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string StatusColor { get; set; } = "#0878D1";
    public bool CanEdit { get; set; } = true;
    public bool CanDelete { get; set; } = true;
    public bool ShowSpecialAction { get; set; }
    public string SpecialActionText { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string ResourceUrl { get; set; } = string.Empty;
}
