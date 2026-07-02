namespace ApiMunicipio.DTO
{
    public class UsuarioSesionDTO
{
    public string Id { get; set; }

    public string NombreCompleto { get; set; }

    public string Email { get; set; }

    public string Cedula { get; set; }

    public string Sector { get; set; }

    public List<string> Roles { get; set; }
}
}
