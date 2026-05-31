using WebMunicipio.Models;
using Newtonsoft.Json;

namespace WebMunicipio.Services
{
    public class ReporteService
    {
        private readonly HttpClient _httpClient;

        public ReporteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Reporte>> ObtenerReporte()
        {
            var response = await _httpClient.GetAsync("api/Reporte");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine(json);


                return JsonConvert.DeserializeObject<List<Reporte>>(json);
            }

            return new List<Reporte>();
        }

        public async Task<bool> CrearReporte(Reporte reporte)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Reporte", reporte);

            return response.IsSuccessStatusCode;
        }

        public async Task<Reporte> ObtenerReporte(int id)
        {
            return await _httpClient.GetFromJsonAsync<Reporte>($"api/Reporte/{id}");
        }

        public async Task<bool> EditarReporte(Reporte reporte)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Reporte/{reporte.IdReporte}", reporte);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarReporte(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Reporte/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
