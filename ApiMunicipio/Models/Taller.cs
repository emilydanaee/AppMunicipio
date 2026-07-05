using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;



namespace ApiMunicipio.Models
{
    
    public class Taller
    {
       

        // MODULO 1: CASA SOMOS

        [Key]
        public int IdTaller { get; set; }
        [Required(ErrorMessage = "El nombre del taller es obligatorio.")]
        public string NombreTaller { get; set; }
        [Required(ErrorMessage = "La descripción del taller es obligatorio.")]
        public string DescripcionTaller { get; set; }
        [Required(ErrorMessage = "Debe ingresar la fecha de inicio")]
        public DateOnly FechaInicio { get; set; }
        [Required(ErrorMessage = "Debe ingresar la fecha de fin")]
        public DateOnly FechaFin { get; set; }
        [Required(ErrorMessage = "Debe ingresar la hora de inicio")]
        public TimeOnly HoraInicio { get; set; }
        [Required(ErrorMessage = "Debe ingresar la hora de fin")]
        public TimeOnly HoraFin { get; set; }
        [Required(ErrorMessage = "Debe seleccionar un sector.")]
        public string SectorTaller { get; set; }
        [Required(ErrorMessage = "Debe ingresar la ubicación.")]
        public string UbicacionTaller { get; set; }
        [Required(ErrorMessage = "Debe ingresar el nombre del instructor.")]
        public string Instructor { get; set; }
        [Required(ErrorMessage = "Debe seleccionar al menos un día.")]
        public string Dias { get; set; }
        [Required(ErrorMessage = "Debe seleccionar una modalidad.")]
        public string Modalidad { get; set; }
        public string? ImagenTaller { get; set; }
        [Required(ErrorMessage = "Debe ingresar la cantidad de cupos.")]
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
