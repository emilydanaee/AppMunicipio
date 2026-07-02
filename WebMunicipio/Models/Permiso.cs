using System.ComponentModel.DataAnnotations;

namespace WebMunicipio.Models
{
    public class Permiso
    {
        public int IdPermiso { get; set; }
        public string TipoPermiso { get; set; }
        public string DescripcionPermiso { get; set; }
        public DateTime FechaPermiso { get; set; }
        public string EstadoPermiso { get; set; }

        public string UsuarioId { get; set; }

    }
}
