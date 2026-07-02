using System.ComponentModel.DataAnnotations;

namespace ApiMunicipio.Models
{
    public class SituacionCalle
    {
        // MODULO 2: REPORTE PERSONA EN SITUACION DE CALLE
        [Key]
        public int IdSituacionCalle { get; set; }

        public string NombreReportante { get; set; }
        public DateTime FechaReporte { get; set; }= DateTime.Now;

        
        public decimal Latitud { get; set; }

        public decimal Longitud { get; set; }

        [Required]
        public string Descripcion { get; set; }

        [Required]
        public string Estado { get; set; }
    }

    
}
