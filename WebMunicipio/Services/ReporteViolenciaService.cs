using WebMunicipio.Models;
using Newtonsoft.Json;

namespace WebMunicipio.Services
{
    public class ReporteViolenciaService
    {
        private readonly HttpClient _httpClient;

        public ReporteViolenciaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<ReporteViolencia>> ObtenerReportesViolencia()
        {
            var response = await _httpClient.GetAsync("api/ReporteViolencia");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine(json);


                return JsonConvert.DeserializeObject<List<ReporteViolencia>>(json);
            }

            return new List<ReporteViolencia>();
        }

        public async Task<bool> CrearReporteViolencia(ReporteViolencia reporteViolencia)
        {
            var response = await _httpClient.PostAsJsonAsync("api/ReporteViolencia", reporteViolencia);

            return response.IsSuccessStatusCode;
        }

        public async Task<ReporteViolencia> ObtenerReporteViolencia(int id)
        {
            return await _httpClient.GetFromJsonAsync<ReporteViolencia>($"api/ReporteViolencia/{id}");
        }

        public async Task<bool> EditarReporteViolencia(ReporteViolencia reporteViolencia)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/ReporteViolencia/{reporteViolencia.IdReporteViolencia}", reporteViolencia);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarReporteViolencia(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/ReporteViolencia/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
