using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMunicipio.Models
{
    public class SituacionCalle
    {
        [Key]
        public int IdSituacionCalle { get; set; }

        [Required]
        public string NombreReportante { get; set; }

        public DateTime FechaReporte { get; set; } = DateTime.Now;

        [Required]
        public string Sector { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal Latitud { get; set; }

        [Column(TypeName = "decimal(18,6)")]
        public decimal Longitud { get; set; }

        public string? Direccion { get; set; }

        [Required]
        public string Condicion { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public string Prioridad { get; set; }

        public bool RiesgoInmediato { get; set; }

        [Required]
        public string Estado { get; set; } = "Pendiente";
    }
}