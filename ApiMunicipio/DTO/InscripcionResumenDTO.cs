namespace ApiMunicipio.DTO;

public sealed class InscripcionResumenDTO
{
    public int IdInscripcion { get; set; }
    public int IdTaller { get; set; }
    public string NombreTaller { get; set; } = string.Empty;
    public string DescripcionTaller { get; set; } = string.Empty;
    public DateOnly FechaInicio { get; set; }
    public TimeOnly HoraInicio { get; set; }
    public TimeOnly HoraFin { get; set; }
    public string UbicacionTaller { get; set; } = string.Empty;
    public string SectorTaller { get; set; } = string.Empty;
    public string Modalidad { get; set; } = string.Empty;
    public DateTime FechaInscripcion { get; set; }
    public string Telefono { get; set; } = string.Empty;
}
