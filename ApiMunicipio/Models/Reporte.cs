using System.ComponentModel.DataAnnotations;


namespace ApiMunicipio.Models
{
    public class ReporteDTO
    {
        // MODULO 2: REPORTE CIUDADANO

        [Key]
        public int IdReporte { get; set; }
        [Required]
        public string TipoReporte { get; set; }
        [Required]
        public string DescripcionReporte { get; set; }
        public DateTime FechaReporte { get; set; } = DateTime.Now;
        public decimal Latitud { get; set; }

        public decimal Longitud { get; set; }
        public string Cedula { get; set; }
        [Required]
        [EmailAddress]
        public string Correo { get; set; }
        [Required]
        public string Telefono { get; set; }
        [Required]
        public string EstadoReporte { get; set; }
        public string ImagenReporte { get; set; }
    }
}
