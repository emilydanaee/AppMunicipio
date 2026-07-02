using Microsoft.AspNetCore.Http;

namespace ApiMunicipio.Services
{
    public interface IImageService
    {
        Task<string?> GuardarImagen(IFormFile? archivo, string carpeta);
    }
}