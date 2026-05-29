using System.ComponentModel.DataAnnotations;

namespace ApiMunicipio.Models
{
    // Permisos Municipales
    public class Permiso
    {
        [Key]
        public int IdPermiso { get; set; }
        public string TipoPermiso { get; set; }
        public string DescripcionPermiso { get; set; }
        public DateTime FechaSolicitud { get; set; }
        public string EstadoPermiso { get; set; }
    }
}
