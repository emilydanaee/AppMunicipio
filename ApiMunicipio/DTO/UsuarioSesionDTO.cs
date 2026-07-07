namespace ApiMunicipio.DTO;

public class UsuarioSesionDTO
{
    public string Id { get; set; } = string.Empty;
    public string NombreCompleto { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Cedula { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;
    public List<string> Roles { get; set; } = [];
}
