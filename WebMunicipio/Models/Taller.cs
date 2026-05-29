using System.ComponentModel.DataAnnotations;

namespace WebMunicipio.Models
{
    public class Taller
    {
        public int IdTaller { get; set; }
        public string NombreTaller { get; set; }
        public string DescripcionTaller { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
        public string SectorTaller { get; set; }
        public int CuposTaller { get; set; }

    }
}
