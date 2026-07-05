using System.ComponentModel.DataAnnotations;

namespace WebMunicipio.DTO
{
    public class AlertaDTO
    {
        [Key]
        public int IdAlerta { get; set; }

        [Required]
        public string TipoAlerta { get; set; }

        [Required]
        public string DescripcionAlerta { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required]
        public string Sector { get; set; }

        [Required]
        public decimal Latitud { get; set; }

        [Required]
        public decimal Longitud { get; set; }
        
        public string? Direccion { get; set; }

        public string EstadoAlerta { get; set; } = "Activa";

        [Required]
        public string Cedula { get; set; }

        [Required]
        public string Correo { get; set; }

        public string? Telefono { get; set; }

        public IFormFile? Imagen { get; set; }
    }
}