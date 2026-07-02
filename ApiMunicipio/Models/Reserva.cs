using System.ComponentModel.DataAnnotations;

namespace ApiMunicipio.Models
{// Reserva de Espacios
    public class Reserva
    {
        // MODULO 3: RESERVA DE ESPACIOS PÚBLICOS
        [Key]
        public int IdReserva { get; set; }
        [Required]
        public string EspacioReserva { get; set; }
        [Required]
        public DateOnly FechaReserva { get; set; }
        [Required]
        public TimeOnly HoraInicio { get; set; }
        [Required]
        public TimeOnly HoraFin { get; set; }
        [Required]
        public string MotivoReserva { get; set; }
        public string EstadoReserva { get; set; }
        public string Observaciones { get; set; }

        public string DocumentoAdjunto { get; set; }

    }
}
