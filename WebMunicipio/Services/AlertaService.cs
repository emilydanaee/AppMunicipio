using WebMunicipio.Models;
using Newtonsoft.Json;

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

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine(json);


                return JsonConvert.DeserializeObject<List<Alerta>>(json);
            }

            return new List<Alerta>();
        }

        public async Task<bool> CrearAlerta(Alerta alerta)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Alerta", alerta);

            return response.IsSuccessStatusCode;
        }

        public async Task<Alerta> ObtenerAlerta(int id)
        {
            return await _httpClient.GetFromJsonAsync<Alerta>($"api/Alerta/{id}");
        }

        public async Task<bool> EditarAlerta(Alerta alerta)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Alerta/{alerta.IdAlerta}", alerta);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarAlerta(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Alerta/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
