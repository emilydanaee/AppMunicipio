using WebMunicipio.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;


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
            var contenido = new MultipartFormDataContent();

            contenido.Add(new StringContent(taller.NombreTaller), "NombreTaller");
            contenido.Add(new StringContent(taller.DescripcionTaller ?? ""), "DescripcionTaller");
            contenido.Add(new StringContent(taller.FechaInicio!.Value.ToString("yyyy-MM-dd")), "FechaInicio");
            contenido.Add(new StringContent(taller.FechaFin!.Value.ToString("yyyy-MM-dd")), "FechaFin");
            contenido.Add(new StringContent(taller.HoraInicio.ToString()), "HoraInicio");
            contenido.Add(new StringContent(taller.HoraFin.ToString()), "HoraFin");
            contenido.Add(new StringContent(taller.SectorTaller), "SectorTaller");
            contenido.Add(new StringContent(taller.UbicacionTaller), "UbicacionTaller");
            contenido.Add(new StringContent(taller.Instructor), "Instructor");
            contenido.Add(new StringContent(taller.Dias), "Dias");
            contenido.Add(new StringContent(taller.Modalidad), "Modalidad");
            contenido.Add(new StringContent(taller.CuposTaller.ToString()), "CuposTaller");

            if (taller.Imagen != null)
            {
                var stream = taller.Imagen.OpenReadStream();

                contenido.Add(
                    new StreamContent(stream),
                    "Imagen",
                    taller.Imagen.FileName);
            }

            var response = await _httpClient.PostAsync("api/Taller", contenido);

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
