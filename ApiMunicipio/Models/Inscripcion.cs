using System.ComponentModel.DataAnnotations;

namespace ApiMunicipio.Models
{
    public class Inscripcion
    {
        // MODULO 1: 

        [Key]
        public int IdInscripcion { get; set; }
        [Required]
        public string NombreActividad { get; set; }
        [Required]
        public string DescripcionActividad { get; set; }
        [Required]
        public string NombreCompleto { get; set; }
        [Required]
        public string Cedula { get; set; }
        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        public string Telefono { get; set; }
        public DateTime FechaInscripcion { get; set; } = DateTime.Now;

        public string Estado { get; set; }
    }
}
