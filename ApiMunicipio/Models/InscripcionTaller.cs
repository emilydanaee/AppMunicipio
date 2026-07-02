namespace ApiMunicipio.Models
{
    public class InscripcionTaller
    {
        public int Id { get; set; }

        public int IdTaller { get; set; }

        public string Nombre { get; set; }

        public string Cedula { get; set; }

        public string Correo { get; set; }

        public string Telefono { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
