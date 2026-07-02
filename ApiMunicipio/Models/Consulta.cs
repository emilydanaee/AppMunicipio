using System.ComponentModel.DataAnnotations;

namespace ApiMunicipio.Models
{
    public class Consulta
    {
        // MODULO 3: CONSULTA DE IMPUESTOS

        [Key]
        public int IdConsulta { get; set; }
        [Required]
        public string Cedula { get; set; }

        public decimal ValorPendiente { get; set; }

        public DateTime FechaConsulta { get; set; } = DateTime.Now;
        public bool Pagado { get; set; }
    }
}
