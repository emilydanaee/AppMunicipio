using System.ComponentModel.DataAnnotations;

namespace WebMunicipio.Models
{
    public class ReporteViolencia
    {
        // MODULO 2: REPORTE VIOLENCIA
        public int IdReporteViolencia { get; set; }
        public string TipoViolencia { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public DateTime FechaReporte { get; set; }
    }
}
