using System.ComponentModel.DataAnnotations;

namespace WebMunicipio.Models
{
    public class Inscripcion
    {
        // MODULO 1

        public int IdInscripcion { get; set; }
        public string NombreActividad { get; set; }
        public string DescripcionActividad { get; set; }
        public string NombreCompleto { get; set; }
        public string Cedula { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaInscripcion { get; set; } = DateTime.Now;
        public string Estado { get; set; }
    }
}
