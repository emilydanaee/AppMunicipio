using System.ComponentModel.DataAnnotations;


namespace ApiMunicipio.Models
{
    public class Campania
    {
        // MODULO 1: CAMPAÑAS DE BIENESTAR ANIMAL

        [Key]
        public int IdCampania { get; set; }
        [Required]
        public string NombreCampania { get; set; }
        [Required]
        public string DescripcionCampania { get; set; }
        [Required]
        public DateTime FechaInicio { get; set; }
        [Required]
        public DateTime FechaFin { get; set; }
        [Required]
        public string SectorCampania { get; set; }
        public string? ImagenCampania { get; set; }
    }
}
