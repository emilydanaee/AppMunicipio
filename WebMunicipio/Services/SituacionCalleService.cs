using Newtonsoft.Json;
using WebMunicipio.Models;

namespace WebMunicipio.Services
{
    public class SituacionCalleService
    {
        private readonly HttpClient _httpClient;

        public SituacionCalleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<List<SituacionCalle>> ObtenerSituaciones()
        {
            var response = await _httpClient.GetAsync("api/SituacionCalle");

            if (!response.IsSuccessStatusCode)
                return new List<SituacionCalle>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<SituacionCalle>>(json);
        }


        public async Task<SituacionCalle> ObtenerSituacion(int id)
        {
            return await _httpClient.GetFromJsonAsync<SituacionCalle>($"api/SituacionCalle/{id}");
        }


        public async Task<bool> CrearSituacion(SituacionCalle situacion)
        {
            var response = await _httpClient.PostAsJsonAsync(
                "api/SituacionCalle",
                situacion);

            return response.IsSuccessStatusCode;
        }


        public async Task<bool> EditarSituacion(SituacionCalle situacion)
        {
            var response = await _httpClient.PutAsJsonAsync(
                $"api/SituacionCalle/{situacion.IdSituacionCalle}",
                situacion);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarEstado(int id, string estado)
        {
            var contenido = JsonContent.Create(new
            {
                Estado = estado
            });

            var response = await _httpClient.PatchAsync(
                $"api/SituacionCalle/{id}/estado",
                contenido);

            return response.IsSuccessStatusCode;
        }


        public async Task<bool> EliminarSituacion(int id)
        {
            var response = await _httpClient.DeleteAsync(
                $"api/SituacionCalle/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}