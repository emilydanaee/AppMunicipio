using WebMunicipio.Models;
using Newtonsoft.Json;

namespace WebMunicipio.Services
{
    public class PermisoService
    {
        private readonly HttpClient _httpClient;

        public PermisoService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Permiso>> ObtenerPermisos()
        {
            var response = await _httpClient.GetAsync("api/Permiso");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine(json);


                return JsonConvert.DeserializeObject<List<Permiso>>(json);
            }

            return new List<Permiso>();
        }

        public async Task<bool> CrearPermiso(Permiso permiso)
        {
            var response = await _httpClient.PostAsJsonAsync("api/Permiso", permiso);

            return response.IsSuccessStatusCode;
        }

        public async Task<Permiso> ObtenerPermiso(int id)
        {
            return await _httpClient.GetFromJsonAsync<Permiso>($"api/Permiso/{id}");
        }

        public async Task<bool> EditarPermiso(Permiso permiso)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Permiso/{permiso.IdPermiso}", permiso);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarPermiso(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Permiso/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
