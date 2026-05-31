using WebMunicipio.Models;
using Newtonsoft.Json;

namespace WebMunicipio.Services
{
    public class SolicitudService
    {
        private readonly HttpClient _httpClient;

        public SolicitudService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Solicitud>> ObtenerSolicitudes()
        {
            var response = await _httpClient.GetAsync("api/Solicitud");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine(json);


                return JsonConvert.DeserializeObject<List<Solicitud>>(json);
            }

            return new List<Solicitud>();
        }

        public async Task<bool> CrearSolicitud(Solicitud solicitud)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Solicitud", solicitud);

            return response.IsSuccessStatusCode;
        }

        public async Task<Solicitud> ObtenerSolicitud(int id)
        {
            return await _httpClient.GetFromJsonAsync<Solicitud>($"api/Solicitud/{id}");
        }

        public async Task<bool> EditarSolicitud(Solicitud solicitud)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Solicitud/{solicitud.IdSolicitud}", solicitud);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarSolicitud(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Solicitud/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
