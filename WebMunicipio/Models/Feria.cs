using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace WebMunicipio.Models
{
    public class Feria
    {
        public int IdFeria { get; set; }
        [Required(ErrorMessage = "El nombre de la feria es obligatorio.")]
        public string NombreFeria { get; set; }
        [Required(ErrorMessage = "La descripción de la feria es obligatoria.")]
        public string DescripcionFeria { get; set; }
        [Required(ErrorMessage = "Debe ingresar la fecha de inicio")]
        public DateOnly? FechaInicio { get; set; }
        [Required(ErrorMessage = "Debe ingresar la fecha de fin")]
        public DateOnly? FechaFin { get; set; }
        [Required(ErrorMessage = "Debe ingresar la hora de inicio")]
        public TimeOnly? HoraInicio { get; set; }
        [Required(ErrorMessage = "Debe ingresar la hora de fin")]
        public TimeOnly? HoraFin { get; set; }
        [Required(ErrorMessage = "Debe seleccionar un sector.")]
        public string SectorFeria { get; set; }
        [Required(ErrorMessage = "Debe ingresar la ubicación.")]
        public string UbicacionFeria { get; set; }
        public string? ImagenFeria { get; set; }
        [NotMapped]
        public IFormFile? Imagen { get; set; }
    }
}
