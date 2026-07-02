using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ApiMunicipio.Models
{
    public class Usuario : IdentityUser 
    {
        [Required]
        public string NombreCompleto { get; set; }
        [Required]
        public string Cedula { get; set; }
        [Required]
        public string Sector { get; set; }
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }

}
