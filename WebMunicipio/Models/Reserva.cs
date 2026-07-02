using System.ComponentModel.DataAnnotations;

namespace WebMunicipio.Models
{// Reserva de Espacios
    public class Reserva
    {
        // MODULO 3: RESERVA DE ESPACIOS PÚBLICOS
        public int IdReserva { get; set; }
        public string EspacioReserva { get; set; }
        public DateOnly FechaReserva { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
        public string EstadoReserva { get; set; }

    }
}
