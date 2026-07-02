using WebMunicipio.Models;
using Newtonsoft.Json;

namespace WebMunicipio.Services
{
    public class ReservaService
    {
        private readonly HttpClient _httpClient;

        public ReservaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Reserva>> ObtenerReservas()
        {
            var response = await _httpClient.GetAsync("api/Reserva");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine(json);


                return JsonConvert.DeserializeObject<List<Reserva>>(json);
            }

            return new List<Reserva>();
        }

        public async Task<bool> CrearReserva(Reserva reserva)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Reserva", reserva);

            return response.IsSuccessStatusCode;
        }

        public async Task<Reserva> ObtenerReserva(int id)
        {
            return await _httpClient.GetFromJsonAsync<Reserva>($"api/Reserva/{id}");
        }

        public async Task<bool> EditarReserva(Reserva reserva)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Reserva/{reserva.IdReserva}", reserva);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarReserva(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Reserva/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
