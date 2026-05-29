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
    }
}
