using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ApiMunicipio.DTO
{
    public class ReporteDTO
    {
        // MODULO 2: REPORTE CIUDADANO

        [Key]
        public int IdReporte { get; set; }
        public string TipoReporte { get; set; }
        public string DescripcionReporte { get; set; }
        public string? Parroquia { get; set; }
        public string? AdministracionZonal { get; set; }
        public DateTime FechaReporte { get; set; } = DateTime.Now;
        [Column(TypeName = "decimal(18,6)")]
        public decimal Latitud { get; set; }
        [Column(TypeName = "decimal(18,6)")]
        public decimal Longitud { get; set; }
        public string? Direccion { get; set; }
        public string EstadoReporte { get; set; } = "Pendiente";
        public string Cedula { get; set; }
        public string Correo { get; set; }
        public string? Telefono { get; set; }
        public IFormFile? Imagen { get; set; }
    }
}
