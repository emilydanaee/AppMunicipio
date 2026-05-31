using WebMunicipio.Models;
using Newtonsoft.Json;

namespace WebMunicipio.Services
{
    public class TallerService
    {
        private readonly HttpClient _httpClient;

        public TallerService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Taller>> ObtenerTalleres()
        {
            var response = await _httpClient.GetAsync("api/Taller");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine(json);


                return JsonConvert.DeserializeObject<List<Taller>>(json);
            }

            return new List<Taller>();
        }

        public async Task<bool> CrearTaller(Taller taller)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Taller", taller);

            return response.IsSuccessStatusCode;
        }

        public async Task<Taller> ObtenerTaller(int id)
        {
            return await _httpClient.GetFromJsonAsync<Taller>($"api/Taller/{id}");
        }

        public async Task<bool> EditarTaller(Taller taller)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Taller/{taller.IdTaller}", taller);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarTaller(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Taller/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
