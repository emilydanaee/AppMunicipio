using System.ComponentModel.DataAnnotations;

namespace ApiMunicipio.Models
{// Reserva de Espacios
    public class Reserva
    {
        [Key]
        public int IdReserva { get; set; }
        public string EspacioPublico { get; set; }
        public DateOnly FechaReserva { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
        public string EstadoReserva { get; set; }
    }
}
