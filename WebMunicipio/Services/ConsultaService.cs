using WebMunicipio.Models;
using Newtonsoft.Json;

namespace WebMunicipio.Services
{
    public class ConsultaService
    {
        private readonly HttpClient _httpClient;

        public ConsultaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Consulta>> ObtenerConsulta()
        {
            var response = await _httpClient.GetAsync("api/Consulta");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine(json);


                return JsonConvert.DeserializeObject<List<Consulta>>(json);
            }

            return new List<Consulta>();
        }

        public async Task<bool> CrearConsulta(Consulta consulta)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Consulta", consulta);

            return response.IsSuccessStatusCode;
        }

        public async Task<Consulta> ObtenerConsulta(int id)
        {
            return await _httpClient.GetFromJsonAsync<Consulta>($"api/Consulta/{id}");
        }

        public async Task<bool> EditarConsulta(Consulta consulta)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Consulta/{consulta.IdConsulta}", consulta);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarConsulta(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Consulta/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
