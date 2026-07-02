using WebMunicipio.Models;
using Newtonsoft.Json;

namespace WebMunicipio.Services
{
    public class SituacionCalleService
    {
        private readonly HttpClient _httpClient;

        public SituacionCalleService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SituacionCalle>> ObtenerSituacionCalle()
        {
            var response = await _httpClient.GetAsync("api/SituacionCalle");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine(json);


                return JsonConvert.DeserializeObject<List<SituacionCalle>>(json);
            }

            return new List<SituacionCalle>();
        }

        public async Task<bool> CrearSituacionCalle(SituacionCalle situacionCalle)
        {
            var response = await _httpClient.PostAsJsonAsync("api/SituacionCalle", situacionCalle);

            return response.IsSuccessStatusCode;
        }

        public async Task<SituacionCalle> ObtenerSituacionCalle(int id)
        {
            return await _httpClient.GetFromJsonAsync<SituacionCalle>($"api/SituacionCalle/{id}");
        }

        public async Task<bool> EditarSituacionCalle(SituacionCalle situacionCalle)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/SituacionCalle/{situacionCalle.IdSituacionCalle}", situacionCalle);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarSituacionCalle(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/SituacionCalle/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
