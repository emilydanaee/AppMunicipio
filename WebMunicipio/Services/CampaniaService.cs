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
            var contenido = new MultipartFormDataContent();

            contenido.Add(new StringContent(campania.NombreCampania), "NombreCampania");
            contenido.Add(new StringContent(campania.DescripcionCampania ?? ""), "DescripcionCampania");
            contenido.Add(new StringContent(campania.FechaInicio!.Value.ToString("yyyy-MM-dd")), "FechaInicio");
            contenido.Add(new StringContent(campania.FechaFin!.Value.ToString("yyyy-MM-dd")), "FechaFin");
            contenido.Add(new StringContent(campania.HoraInicio.ToString()), "HoraInicio");
            contenido.Add(new StringContent(campania.HoraFin.ToString()), "HoraFin");
            contenido.Add(new StringContent(campania.ResponsableCampania), "ResponsableCampania");
            contenido.Add(new StringContent(campania.UbicacionCampania), "UbicacionCampania");
            contenido.Add(new StringContent(campania.TipoCampania), "TipoCampania");

            if (campania.Imagen != null)
            {
                var stream = campania.Imagen.OpenReadStream();

                contenido.Add(
                    new StreamContent(stream),
                    "Imagen",
                    campania.Imagen.FileName);
            }

            var response = await _httpClient.PostAsync("api/Campania", contenido);

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
