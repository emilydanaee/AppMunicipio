using WebMunicipio.Models;
using Newtonsoft.Json;

namespace WebMunicipio.Services
{
    public class CampaniaService
    {
        private readonly HttpClient _httpClient;

        public CampaniaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Campania>> ObtenerCampania()
        {
            var response = await _httpClient.GetAsync("api/Campania");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine(json);


                return JsonConvert.DeserializeObject<List<Campania>>(json);
            }

            return new List<Campania>();
        }

        public async Task<bool> CrearCampania(Campania campania)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Campania", campania);

            return response.IsSuccessStatusCode;
        }

        public async Task<Campania> ObtenerCampania(int id)
        {
            return await _httpClient.GetFromJsonAsync<Campania>($"api/Campania/{id}");
        }

        public async Task<bool> EditarCampania(Campania campania)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Campania/{campania.IdCampania}", campania);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarCampania(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Campania/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
