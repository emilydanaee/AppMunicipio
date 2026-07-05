using Newtonsoft.Json;
using System.Globalization;
using WebMunicipio.Models;

namespace WebMunicipio.Services
{
    public class ReporteService
    {
        private readonly HttpClient _httpClient;

        public ReporteService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Reporte>> ObtenerReporte()
        {
            var response = await _httpClient.GetAsync("api/Reporte");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                Console.WriteLine(json);


                return JsonConvert.DeserializeObject<List<Reporte>>(json);
            }

            return new List<Reporte>();
        }

        public async Task<bool> CrearReporte(Reporte reporte)
        {
            var contenido = new MultipartFormDataContent();

            contenido.Add(new StringContent(reporte.TipoReporte),"TipoReporte");
            contenido.Add(new StringContent(reporte.DescripcionReporte),"DescripcionReporte");
            contenido.Add(new StringContent(reporte.AdministracionZonal), "AdministracionZonal");
            contenido.Add(new StringContent(reporte.Parroquia), "Parroquia");
            contenido.Add(new StringContent(reporte.Latitud.ToString(CultureInfo.InvariantCulture)),"Latitud");
            contenido.Add(new StringContent(reporte.Longitud.ToString(CultureInfo.InvariantCulture)),"Longitud");
            contenido.Add(new StringContent(reporte.Cedula),"Cedula");
            contenido.Add(new StringContent(reporte.Correo),"Correo");
            contenido.Add(new StringContent(reporte.Telefono ?? ""),"Telefono");

            if (!string.IsNullOrWhiteSpace(reporte.Direccion))
            {
                contenido.Add( new StringContent(reporte.Direccion), "Direccion");
            }

            if (reporte.Imagen != null)
            {
                var stream = reporte.Imagen.OpenReadStream();

                contenido.Add(
                    new StreamContent(stream),
                    "Imagen",
                    reporte.Imagen.FileName);
            }

            var response = await _httpClient.PostAsync("api/Reporte", contenido);

            return response.IsSuccessStatusCode;
        }

        public async Task<Reporte> ObtenerReporte(int id)
        {
            return await _httpClient.GetFromJsonAsync<Reporte>($"api/Reporte/{id}");
        }

        public async Task<bool> EditarReporte(Reporte reporte)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/Reporte/{reporte.IdReporte}", reporte);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> EliminarReporte(int id)
        {
            var response = await _httpClient.DeleteAsync($"api/Reporte/{id}");

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ActualizarEstado(int id, string estado)
        {
            var contenido = JsonContent.Create(new
            {
                EstadoReporte = estado
            });

            var response = await _httpClient.PatchAsync(
                $"api/Reporte/{id}/estado",
                contenido);

            return response.IsSuccessStatusCode;
        }
    }
}
