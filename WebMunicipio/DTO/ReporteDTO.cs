using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebMunicipio.DTO
{
    public class ReporteDTO
    {
        // MODULO 2: REPORTE CIUDADANO

        [Key]
        public int IdReporte { get; set; }
        [Required(ErrorMessage = "Debe seleccionar un tipo.")]
        public string TipoReporte { get; set; }
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        public string DescripcionReporte { get; set; }
        [Required]
        public DateTime FechaReporte { get; set; } = DateTime.Now;
        [Column(TypeName = "decimal(18,6)")]
        public decimal Latitud { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal Longitud { get; set; }
        public string? Parroquia { get; set; }
        public string? AdministracionZonal { get; set; }
        public string? Direccion { get; set; }
        [Required]
        public string EstadoReporte { get; set; } = "Pendiente";
        [Required]
        public string Cedula { get; set; }
        [Required]
        public string Correo { get; set; }
        public string? Telefono { get; set; }
        public IFormFile? Imagen { get; set; }

    }
}
