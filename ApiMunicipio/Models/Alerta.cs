using System.ComponentModel.DataAnnotations;

namespace ApiMunicipio.Models
{
    public class Alerta
    {
        // MODULO 2: ALERTAS DE SEGURIDAD

        [Key]
        public int IdAlerta { get; set; }
        [Required]
        public string TipoAlerta { get; set; }
        [Required]
        public string DescripcionAlerta { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;
        public string Sector { get; set; }
        public decimal Latitud { get; set; }

        public decimal Longitud { get; set; }
        [Required]
        public string Cedula { get; set; }
        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        public string Telefono { get; set; }

        public string EstadoAlerta { get; set; }
    }
}
