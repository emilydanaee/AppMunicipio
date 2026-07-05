using Newtonsoft.Json;
using WebMunicipio.Models;

namespace WebMunicipio.Services
{
    public class AlertaService
    {
        private readonly HttpClient _httpClient;

        public AlertaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Alerta>> ObtenerAlertas()
        {
            var response = await _httpClient.GetAsync("api/Alerta");

            if (!response.IsSuccessStatusCode)
                return new List<Alerta>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Alerta>>(json);
        }

        public async Task<Alerta> ObtenerAlerta(int id)
        {
            return await _httpClient.GetFromJsonAsync<Alerta>($"api/Alerta/{id}");
        }

        public async Task<bool> CrearAlerta(Alerta alerta)
        {
            var contenido = new MultipartFormDataContent();

            contenido.Add(new StringContent(alerta.TipoAlerta), "TipoAlerta");
            contenido.Add(new StringContent(alerta.DescripcionAlerta), "DescripcionAlerta");
            contenido.Add(new StringContent(alerta.Sector), "Sector");
            contenido.Add(new StringContent(alerta.Latitud.ToString()), "Latitud");
            contenido.Add(new StringContent(alerta.Longitud.ToString()), "Longitud");

            contenido.Add(new StringContent(alerta.Cedula), "Cedula");
            contenido.Add(new StringContent(alerta.Correo), "Correo");
            contenido.Add(new StringContent(alerta.Telefono ?? ""), "Telefono");

            if (!string.IsNullOrWhiteSpace(alerta.Direccion))
                contenido.Add(new StringContent(alerta.Direccion), "Direccion");

            if (alerta.Imagen != null)
            {
                contenido.Add(
                    new StreamContent(alerta.Imagen.OpenReadStream()),
                    "Imagen",
                    alerta.Imagen.FileName);
            }

            var response = await _httpClient.PostAsync("api/Alerta", contenido);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarEstado(int id, string estado)
        {
            var alerta = new Alerta
            {
                EstadoAlerta = estado
            };

            var response = await _httpClient.PutAsJsonAsync(
                $"api/Alerta/{id}",
                alerta);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarAlerta(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Alerta/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}