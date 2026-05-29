using System.ComponentModel.DataAnnotations;

namespace WebMunicipio.Models
{
    public class Reporte
    {
        public int IdReporte { get; set; }
        public string TipoReporte { get; set; }
        public string DescripcionReporte { get; set; }
        public DateTime FechaReporte { get; set; }
        public string UbicacionReporte { get; set; }
        public string EstadoReporte { get; set; }
    }
}
