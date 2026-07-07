using System.ComponentModel.DataAnnotations;

namespace ApiMunicipio.Models;

public class Reserva
{
    [Key]
    public int IdReserva { get; set; }

    [Required]
    public string EspacioReserva { get; set; } = string.Empty;

    [Required]
    public DateOnly FechaReserva { get; set; }

    [Required]
    public TimeOnly HoraInicio { get; set; }

    [Required]
    public TimeOnly HoraFin { get; set; }

    [Required]
    public string MotivoReserva { get; set; } = string.Empty;

    public string EstadoReserva { get; set; } = "Solicitada";
    public string Observaciones { get; set; } = string.Empty;
    public string DocumentoAdjunto { get; set; } = string.Empty;
}
