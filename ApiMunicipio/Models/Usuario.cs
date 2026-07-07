using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace ApiMunicipio.Models;

public class Usuario : IdentityUser
{
    [Required]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required]
    public string Cedula { get; set; } = string.Empty;

    [Required]
    public string Sector { get; set; } = string.Empty;

    public DateTime FechaRegistro { get; set; } = DateTime.Now;
}
