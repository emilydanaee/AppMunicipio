using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace ApiMunicipio.Models
{
    
    public class Taller
    {
        // MODULO 1: CASA SOMOS

        [Key]
        public int IdTaller { get; set; }
        [Required]
        public string NombreTaller { get; set; }

        public string DescripcionTaller { get; set; }
        [Required]
        public DateOnly FechaInicio { get; set; }
        [Required]
        public DateOnly FechaFin { get; set; }
        [Required]
        public TimeOnly HoraInicio { get; set; }
        [Required]
        public TimeOnly HoraFin { get; set; }
        [Required]
        public string SectorTaller { get; set; }
        [Required]
        public string Instructor { get; set; }
        [Required]
        public string Dias { get; set; }
        [Required]
        public string Modalidad { get; set; }
        public string? ImagenTaller { get; set; }
        public int CuposTaller { get; set; }
        public int Inscritos { get; set; } = 0;

        [NotMapped]
        public int CuposDisponibles
        {
            get
            {
                return CuposTaller - Inscritos;
            }
        }
    }

}
