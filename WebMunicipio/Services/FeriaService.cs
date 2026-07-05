using WebMunicipio.Models;
using Newtonsoft.Json;

namespace WebMunicipio.Services
{
    public class FeriaService
    {
        private readonly HttpClient _httpClient;

        public FeriaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Feria>> ObtenerFerias()
        {
            var response = await _httpClient.GetAsync("api/Feria");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine(json);


                return JsonConvert.DeserializeObject<List<Feria>>(json);
            }

            return new List<Feria>();
        }

        public async Task<bool> CrearFeria(Feria feria)
        {
            var contenido = new MultipartFormDataContent();

            contenido.Add(new StringContent(feria.NombreFeria), "NombreFeria");
            contenido.Add(new StringContent(feria.DescripcionFeria ?? ""), "DescripcionFeria");
            contenido.Add(new StringContent(feria.FechaInicio!.Value.ToString("yyyy-MM-dd")), "FechaInicio");
            contenido.Add(new StringContent(feria.FechaFin!.Value.ToString("yyyy-MM-dd")), "FechaFin");
            contenido.Add(new StringContent(feria.HoraInicio.ToString()), "HoraInicio");
            contenido.Add(new StringContent(feria.HoraFin.ToString()), "HoraFin");
            contenido.Add(new StringContent(feria.SectorFeria), "SectorFeria");
            contenido.Add(new StringContent(feria.UbicacionFeria), "UbicacionFeria");
            

            if (feria.Imagen != null)
            {
                var stream = feria.Imagen.OpenReadStream();

                contenido.Add(
                    new StreamContent(stream),
                    "Imagen",
                    feria.Imagen.FileName);
            }

            var response = await _httpClient.PostAsync("api/Feria", contenido);

            return response.IsSuccessStatusCode;
        }


        public async Task<Feria> ObtenerFeria(int id)
        {
            return await _httpClient.GetFromJsonAsync<Feria>($"api/Feria/{id}");
        }

        public async Task<bool> EditarFeria(Feria feria)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Feria/{feria.IdFeria}", feria);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarFeria(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Feria/{id}");

            return response.IsSuccessStatusCode;
        }
    }
}
