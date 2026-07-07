using System.ComponentModel.DataAnnotations;

namespace ApiMunicipio.DTO;

public class InscripcionTallerDTO
{
    [Range(1, int.MaxValue)]
    public int IdTaller { get; set; }

    [Required]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public string Cedula { get; set; } = string.Empty;

    [Required, EmailAddress]
    public string Correo { get; set; } = string.Empty;

    [Required]
    public string Telefono { get; set; } = string.Empty;
}
