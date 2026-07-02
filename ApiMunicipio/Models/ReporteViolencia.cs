using System.ComponentModel.DataAnnotations;

namespace ApiMunicipio.Models
{
    public class ReporteViolencia
    {
        // MODULO 2: REPORTE VIOLENCIA
        [Key]
        public int IdReporteViolencia { get; set; }
        [Required]
        public string TipoViolencia { get; set; }
        [Required]
        public string Descripcion { get; set; }
        public decimal Latitud { get; set; }

        public decimal Longitud { get; set; }
        public string Referencia { get; set; }
        public string Cedula { get; set; }
        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        public string Telefono { get; set; }
        [Required]
        public string Estado { get; set; }
        [Required]
        public DateTime FechaReporte { get; set; }
    }
}
