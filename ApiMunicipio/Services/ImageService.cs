using Microsoft.AspNetCore.Http;

namespace ApiMunicipio.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _env;

        public ImageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string?> GuardarImagen(IFormFile? archivo, string carpeta)
        {
            if (archivo == null || archivo.Length == 0)
                return null;

            string nombre =
                Guid.NewGuid().ToString() +
                Path.GetExtension(archivo.FileName);

            string rutaCarpeta =
                Path.Combine(_env.WebRootPath, "uploads", carpeta);

            if (!Directory.Exists(rutaCarpeta))
                Directory.CreateDirectory(rutaCarpeta);

            string rutaCompleta =
                Path.Combine(rutaCarpeta, nombre);

            using var stream = new FileStream(rutaCompleta, FileMode.Create);

            await archivo.CopyToAsync(stream);

            return $"uploads/{carpeta}/{nombre}";
        }
    }
}