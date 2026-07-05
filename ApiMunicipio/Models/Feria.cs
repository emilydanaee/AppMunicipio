using System.ComponentModel.DataAnnotations;

namespace ApiMunicipio.Models
{
    public class Feria
    {
        // MODULO 1: FERIAS

        [Key]
        public int IdFeria { get; set; }
        [Required(ErrorMessage = "El nombre de la feria es obligatorio.")]
        public string NombreFeria { get; set; }
        [Required]
        public string DescripcionFeria { get; set; }
        [Required]
        public DateOnly FechaInicio { get; set; }
        [Required]
        public DateOnly FechaFin { get; set; }
        [Required]
        public TimeOnly HoraInicio { get; set; }
        [Required]
        public TimeOnly HoraFin { get; set; }

        [Required]
        public string SectorFeria { get; set; }
        [Required]

        public string? ImagenFeria { get; set; }
        public string UbicacionFeria { get; set; }
    }
}
