using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ApiMunicipio.Models
{
    public class Usuario : IdentityUser 
    {
        public string NombreCompleto { get; set; }
    }

}
