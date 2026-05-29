using System.ComponentModel.DataAnnotations;

namespace WebMunicipio.Models
{
    public class Feria
    {
        public int IdFeria { get; set; }
        public string NombreFeria { get; set; }
        public string DescripcionFeria { get; set; }
        public DateOnly FechaFeria { get; set; }

        public string SectorFeria { get; set; }
        public int NumeroEmprendedores { get; set; }

    }
}
