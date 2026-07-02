using System.ComponentModel.DataAnnotations;

namespace ApiMunicipio.DTO
{
    public class TallerDTO
    {
        // MODULO 1: CASA SOMOS

        [Key]
        public int IdTaller { get; set; }
        public string NombreTaller { get; set; }
        public string DescripcionTaller { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }

        [Required]
        public string SectorTaller { get; set; }
        public string Instructor { get; set; }
        public string Dias { get; set; }
        public string Modalidad { get; set; }
        public IFormFile? Imagen { get; set; }
        public int CuposTaller { get; set; }
        

    }
}
