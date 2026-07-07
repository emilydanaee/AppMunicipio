using Microsoft.AspNetCore.Mvc;

namespace ApiMunicipio.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DocumentoController : ControllerBase
{
    private static readonly HashSet<string> AllowedExtensions =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ".pdf",
            ".doc",
            ".docx",
            ".jpg",
            ".jpeg",
            ".png"
        };

    [HttpPost]
    [Consumes("multipart/form-data")]
    [RequestSizeLimit(10_000_000)]
    public async Task<IActionResult> Subir(
        IFormFile archivo,
        CancellationToken cancellationToken)
    {
        if (archivo is null || archivo.Length == 0)
        {
            return BadRequest(new
            {
                message = "Selecciona un documento."
            });
        }

        var extension = Path.GetExtension(archivo.FileName);

        if (string.IsNullOrWhiteSpace(extension) ||
            !AllowedExtensions.Contains(extension))
        {
            return BadRequest(new
            {
                message = "Solo se admiten archivos PDF, Word, JPG y PNG."
            });
        }

        var folder = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            "uploads",
            "documentos");

        Directory.CreateDirectory(folder);

        var safeName = $"{Guid.NewGuid():N}{extension.ToLowerInvariant()}";
        var physicalPath = Path.Combine(folder, safeName);

        await using (var stream = System.IO.File.Create(physicalPath))
        {
            await archivo.CopyToAsync(stream, cancellationToken);
        }

        return Ok(new
        {
            message = "Documento subido correctamente.",
            path = $"uploads/documentos/{safeName}",
            fileName = Path.GetFileName(archivo.FileName)
        });
    }
}