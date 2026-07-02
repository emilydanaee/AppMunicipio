using System.ComponentModel.DataAnnotations;

namespace WebMunicipio.Models
{
    public class Feria
    {
        public int IdFeria { get; set; }
        public string NombreFeria { get; set; }
        public string DescripcionFeria { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public TimeOnly HoraInicio { get; set; }
        
        public TimeOnly HoraFin { get; set; }
        public string SectorFeria { get; set; }
        public string DireccionFeria { get; set; }
        public string? ImagenFeria { get; set; }
    }
}
