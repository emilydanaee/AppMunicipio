using System.ComponentModel.DataAnnotations;

namespace ApiMunicipio.DTO;

public class RegistroDTO
{
    [Required]
    public string NombreCompleto { get; set; } = string.Empty;

    [Required]
    public string Cedula { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required, MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    public string Sector { get; set; } = string.Empty;
}
