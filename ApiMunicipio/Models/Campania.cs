using System.ComponentModel.DataAnnotations;


namespace ApiMunicipio.Models
{
    public class Campania
    {
        // MODULO 1: CAMPAÑAS DE BIENESTAR ANIMAL

        [Key]
        public int IdCampania { get; set; }
        public string NombreCampania { get; set; }
        public string TipoCampania { get; set; }
        public string DescripcionCampania { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }
        public string UbicacionCampania { get; set; }
        public string ResponsableCampania { get; set; }
        public string? ImagenCampania { get; set; }
    }
}
