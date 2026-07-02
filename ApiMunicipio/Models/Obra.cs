using System.ComponentModel.DataAnnotations;

namespace ApiMunicipio.Models
{
    public class Obra
    {
        [Key]
        public int IdObra { get; set; }
        [Required]
        public string NombreObra { get; set; }
        [Required]
        public string DescripcionObra { get; set; }
        [Required]
        public string SectorObra { get; set; }

        public decimal Latitud { get; set; }

        public decimal Longitud { get; set; }
        [Required]
        public DateTime FechaSolicitud { get; set; } = DateTime.Now;

        public string EstadoObra { get; set; }
    }
}
