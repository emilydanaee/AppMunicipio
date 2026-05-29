using System.ComponentModel.DataAnnotations;

namespace WebMunicipio.Models
{
    public class Solicitud
    {
        public int IdSolicitud { get; set; }
        public string TipoSolicitud { get; set; }
        public string DescripcionSolicitud { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string EstadoSolicitud { get; set; }
    }
}
