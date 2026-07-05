using System.ComponentModel.DataAnnotations;

namespace ApiMunicipio.DTO
{
    public class SituacionCalleDTO
    {
        public int IdSituacionCalle { get; set; }

        [Required(ErrorMessage = "Ingrese el nombre del reportante.")]
        public string NombreReportante { get; set; }

        public DateTime FechaReporte { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Seleccione un sector.")]
        public string Sector { get; set; }

        [Required]
        public decimal Latitud { get; set; }

        [Required]
        public decimal Longitud { get; set; }

        public string? Direccion { get; set; }

        [Required(ErrorMessage = "Seleccione la condición observada.")]
        public string Condicion { get; set; }

        [Required(ErrorMessage = "Ingrese una descripción.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "Seleccione una prioridad.")]
        public string Prioridad { get; set; }

        public bool RiesgoInmediato { get; set; }

        public string Estado { get; set; } = "Pendiente";
    }
}