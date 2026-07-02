using System.ComponentModel.DataAnnotations;

namespace WebMunicipio.Models
{
    public class Alerta
    {
        // MODULO 2: ALERTAS DE SEGURIDAD

        public int IdAlerta { get; set; }
        public string TituloAlerta { get; set; }
        public string DescripcionAlerta { get; set; }
        public DateTime Fecha { get; set; }
        public string EstadoAlerta { get; set; }
    }
}
