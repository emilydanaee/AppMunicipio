using System.ComponentModel.DataAnnotations;

namespace ApiMunicipio.Models;

public class Permiso
{
    [Key]
    public int IdPermiso { get; set; }

    [Required]
    public string TipoPermiso { get; set; } = string.Empty;

    public string DescripcionPermiso { get; set; } = string.Empty;
    public DateTime FechaPermiso { get; set; } = DateTime.Now;
    public string EstadoPermiso { get; set; } = "Ingresado";
    public string DocumentoAdjunto { get; set; } = string.Empty;
    public string Observaciones { get; set; } = string.Empty;
}
