using System.ComponentModel.DataAnnotations;

namespace WebMunicipio.Models
{
    public class Consulta
    {
        // MODULO 3: CONSULTA DE IMPUESTOS

        public int IdConsulta { get; set; }
        public string Cedula { get; set; }
        public decimal ValorPendiente { get; set; }
        public DateTime FechaConsulta { get; set; }
    }
}
