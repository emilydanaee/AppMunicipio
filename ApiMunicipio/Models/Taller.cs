using System.ComponentModel.DataAnnotations;

namespace ApiMunicipio.Models
{
    public class Taller
    {
        [Key]
        public int IdTaller { get; set; }

        [Required]
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
