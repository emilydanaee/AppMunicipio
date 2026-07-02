using System.ComponentModel.DataAnnotations;

namespace WebMunicipio.Models
{
    public class SituacionCalle
    {
        // MODULO 2: REPORTE PERSONA EN SITUACION DE CALLE
        public int IdSituacionCalle { get; set; }
        public string NombreReportado { get; set; }
        public string Ubicacion { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
    }

    
}
