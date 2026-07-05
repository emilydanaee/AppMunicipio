using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebMunicipio.Models
{
    public class Alerta
    {
        [Key]
        public int IdAlerta { get; set; }

        [Required(ErrorMessage = "Seleccione el tipo de alerta.")]
        public string TipoAlerta { get; set; }

        [Required(ErrorMessage = "Ingrese una descripción.")]
        public string DescripcionAlerta { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Seleccione el sector.")]
        public string Sector { get; set; }

        [Required]
        public decimal Latitud { get; set; }

        [Required]
        public decimal Longitud { get; set; }

        public string? Direccion { get; set; }

        [Required]
        public string EstadoAlerta { get; set; } = "Activa";

        [Required]
        public string Cedula { get; set; }

        [Required]
        public string Correo { get; set; }

        public string? Telefono { get; set; }

        public string? ImagenAlerta { get; set; }

        [NotMapped]
        public IFormFile? Imagen { get; set; }
    }
}