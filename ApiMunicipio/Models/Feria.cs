using System.ComponentModel.DataAnnotations;

namespace ApiMunicipio.Models
{
    public class Feria
    {
        [Key]
        public int IdFeria { get; set; }
        [Required]
        public string NombreFeria { get; set; }
        public string DescripcionFeria { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin { get; set; }

        [Required]
        public string SectorFeria { get; set; }
        public int NumeroEmprendedores { get; set; }

    }
}
