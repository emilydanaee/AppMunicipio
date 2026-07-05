using System.ComponentModel.DataAnnotations;


namespace WebMunicipio.DTO
{
    public class CampaniaDTO
    {
        // MODULO 1: CAMPAÑAS DE BIENESTAR ANIMAL

        [Key]
        public int IdCampania { get; set; }
        [Required(ErrorMessage = "El nombre de la campaña es obligatorio.")]
        public string NombreCampania { get; set; }
        [Required(ErrorMessage = "La descripción de la campaña es obligatoria.")]
        public string DescripcionCampania { get; set; }
        [Required(ErrorMessage = "El tipo de campaña es obligatorio.")]
        public string TipoCampania { get; set; }
        [Required(ErrorMessage = "Debe ingresar la fecha de inicio")]
        public DateOnly? FechaInicio { get; set; }
        [Required(ErrorMessage = "Debe ingresar la fecha de fin")]
        public DateOnly? FechaFin { get; set; }
        [Required(ErrorMessage = "Debe ingresar la hora de inicio")]
        public TimeOnly? HoraInicio { get; set; }
        [Required(ErrorMessage = "Debe ingresar la hora de fin")]
        public TimeOnly? HoraFin { get; set; }
        [Required(ErrorMessage = "Debe ingresar la ubicacion.")]
        public string UbicacionCampania { get; set; }
        [Required(ErrorMessage = "Debe ingresar el responsable.")]
        public string ResponsableCampania { get; set; }
        public IFormFile? Imagen { get; set; }
    }
}
