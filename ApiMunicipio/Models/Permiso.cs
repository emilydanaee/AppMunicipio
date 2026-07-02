using System.ComponentModel.DataAnnotations;

namespace ApiMunicipio.Models
{
    // Permisos Municipales
    public class Permiso
    {
        // MODULO 3: PERMISOS MUNICIPALES
        [Key]
        public int IdPermiso { get; set; }
        [Required]
        public string TipoPermiso { get; set; }
        public string DescripcionPermiso { get; set; }
        public DateTime FechaPermiso { get; set; } = DateTime.Now;
        public string EstadoPermiso { get; set; }

        public string DocumentoAdjunto { get; set; }

        public string Observaciones { get; set; }

    }
}
