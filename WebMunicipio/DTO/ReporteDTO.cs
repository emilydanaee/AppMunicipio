using System.ComponentModel.DataAnnotations;


namespace WebMunicipio.DTO
{
    public class ReporteDTO
    {
        // MODULO 2: REPORTE CIUDADANO

        
        public int IdReporte { get; set; }
        public string TipoReporte { get; set; }
        public string DescripcionReporte { get; set; }
        public DateTime FechaReporte { get; set; }
        public decimal Latitud { get; set; }

        public decimal Longitud { get; set; }
        public string Cedula { get; set; }

        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string EstadoReporte { get; set; }
        public IFormFile? Imagen { get; set; }
    }
}
