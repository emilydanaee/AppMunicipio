using System.ComponentModel.DataAnnotations;


namespace ApiMunicipio.DTO
{
    public class CampaniaDTO
    {
        // MODULO 1: CAMPAÑAS DE BIENESTAR ANIMAL

        
        public int IdCampania { get; set; }
        
        public string NombreCampania { get; set; }

        public string DescripcionCampania { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public string SectorCampania { get; set; }
        public IFormFile? Imagen { get; set; }
    }
}
