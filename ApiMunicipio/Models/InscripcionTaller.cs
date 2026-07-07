namespace ApiMunicipio.Models;

public class InscripcionTaller
{
    public int Id { get; set; }
    public int IdTaller { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Cedula { get; set; } = string.Empty;
    public string Correo { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public DateTime Fecha { get; set; } = DateTime.Now;
}
