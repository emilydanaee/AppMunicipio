using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace MauiMunicipio.Services;

public sealed class ApiService
{
    private readonly HttpClient _httpClient = new()
    {
        Timeout = TimeSpan.FromSeconds(40)
    };

    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public Task<ApiResult<List<T>>> GetListAsync<T>(string endpoint, CancellationToken cancellationToken = default) =>
        SendAsync<List<T>>(HttpMethod.Get, endpoint, null, cancellationToken);

    public Task<ApiResult<T>> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default) =>
        SendAsync<T>(HttpMethod.Get, endpoint, null, cancellationToken);

    public Task<ApiResult<TResponse>> PostJsonAsync<TRequest, TResponse>(
        string endpoint,
        TRequest request,
        CancellationToken cancellationToken = default) =>
        SendJsonAsync<TRequest, TResponse>(HttpMethod.Post, endpoint, request, cancellationToken);

    public Task<ApiResult<TResponse>> PutJsonAsync<TRequest, TResponse>(
        string endpoint,
        TRequest request,
        CancellationToken cancellationToken = default) =>
        SendJsonAsync<TRequest, TResponse>(HttpMethod.Put, endpoint, request, cancellationToken);

    public Task<ApiResult<TResponse>> PatchJsonAsync<TRequest, TResponse>(
        string endpoint,
        TRequest request,
        CancellationToken cancellationToken = default) =>
        SendJsonAsync<TRequest, TResponse>(HttpMethod.Patch, endpoint, request, cancellationToken);

    public Task<ApiResult<object>> DeleteAsync(string endpoint, CancellationToken cancellationToken = default) =>
        SendAsync<object>(HttpMethod.Delete, endpoint, null, cancellationToken);

    public async Task<ApiResult<TResponse>> PostMultipartAsync<TResponse>(
        string endpoint,
        IReadOnlyDictionary<string, string?> fields,
        FileResult? file = null,
        string fileFieldName = "Imagen",
        CancellationToken cancellationToken = default)
    {
        using var content = new MultipartFormDataContent();
        foreach (var field in fields)
            content.Add(new StringContent(field.Value ?? string.Empty), field.Key);

        Stream? stream = null;
        try
        {
            if (file is not null)
            {
                stream = await file.OpenReadAsync();
                var fileContent = new StreamContent(stream);
                fileContent.Headers.ContentType = new MediaTypeHeaderValue(
                    string.IsNullOrWhiteSpace(file.ContentType)
                        ? "application/octet-stream"
                        : file.ContentType);
                content.Add(fileContent, fileFieldName, file.FileName);
            }

            return await SendAsync<TResponse>(HttpMethod.Post, endpoint, content, cancellationToken);
        }
        finally
        {
            stream?.Dispose();
        }
    }

    public string BuildImageUrl(string? path)
    {
        if (string.IsNullOrWhiteSpace(path))
            return string.Empty;
        if (Uri.TryCreate(path, UriKind.Absolute, out _))
            return path;
        return $"{ApiSettings.BaseUrl}/{path.TrimStart('/')}";
    }

    private async Task<ApiResult<TResponse>> SendJsonAsync<TRequest, TResponse>(
        HttpMethod method,
        string endpoint,
        TRequest request,
        CancellationToken cancellationToken)
    {
        var json = JsonSerializer.Serialize(request, _jsonOptions);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");
        return await SendAsync<TResponse>(method, endpoint, content, cancellationToken);
    }

    private async Task<ApiResult<T>> SendAsync<T>(
        HttpMethod method,
        string endpoint,
        HttpContent? content,
        CancellationToken cancellationToken)
    {
        var url = $"{ApiSettings.BaseUrl}/{endpoint.TrimStart('/')}";
        using var request = new HttpRequestMessage(method, url) { Content = content };
        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        try
        {
            using var response = await _httpClient.SendAsync(request, cancellationToken);
            var body = await response.Content.ReadAsStringAsync(cancellationToken);

            if (!response.IsSuccessStatusCode)
                return ApiResult<T>.Failure(ExtractError(body, response.ReasonPhrase), (int)response.StatusCode);

            if (string.IsNullOrWhiteSpace(body))
                return ApiResult<T>.Success(default, (int)response.StatusCode);

            try
            {
                var data = JsonSerializer.Deserialize<T>(body, _jsonOptions);
                return ApiResult<T>.Success(data, (int)response.StatusCode);
            }
            catch (JsonException)
            {
                // Algunos endpoints exitosos devuelven texto simple o no requieren respuesta tipada.
                return ApiResult<T>.Success(default, (int)response.StatusCode);
            }
        }
        catch (TaskCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            return ApiResult<T>.Failure(
                "La API tardó demasiado en responder. Revisa que siga ejecutándose y que la dirección sea correcta.");
        }
        catch (HttpRequestException ex)
        {
            return ApiResult<T>.Failure($"No se pudo conectar con la API: {ex.Message}");
        }
        catch (Exception ex)
        {
            return ApiResult<T>.Failure($"Ocurrió un error inesperado: {ex.Message}");
        }
    }

    private static string ExtractError(string body, string? reasonPhrase)
    {
        if (string.IsNullOrWhiteSpace(body))
            return reasonPhrase ?? "La solicitud no pudo completarse.";

        try
        {
            using var document = JsonDocument.Parse(body);
            var root = document.RootElement;

            foreach (var property in new[] { "message", "error", "title" })
            {
                if (TryGetPropertyIgnoreCase(root, property, out var value) &&
                    value.ValueKind == JsonValueKind.String &&
                    !string.IsNullOrWhiteSpace(value.GetString()))
                {
                    return value.GetString()!;
                }
            }

            if (TryGetPropertyIgnoreCase(root, "errors", out var errors) &&
                errors.ValueKind == JsonValueKind.Object)
            {
                foreach (var errorProperty in errors.EnumerateObject())
                {
                    if (errorProperty.Value.ValueKind == JsonValueKind.Array)
                    {
                        var messages = errorProperty.Value.EnumerateArray()
                            .Select(item => item.ToString())
                            .Where(message => !string.IsNullOrWhiteSpace(message));
                        var joined = string.Join(" ", messages);
                        if (!string.IsNullOrWhiteSpace(joined))
                            return joined;
                    }
                }
            }
        }
        catch (JsonException)
        {
            // El cuerpo no era JSON; se devolverá como texto.
        }

        if (body.Contains("SqlException", StringComparison.OrdinalIgnoreCase) ||
            body.Contains("Could not open a connection to SQL Server", StringComparison.OrdinalIgnoreCase))
        {
            return "La API está abierta, pero perdió la conexión con LocalDB. Reinicia únicamente la API; la aplicación volverá a conectarse automáticamente.";
        }

        return body.Length > 500 ? body[..500] : body;
    }

    private static bool TryGetPropertyIgnoreCase(JsonElement element, string name, out JsonElement value)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in element.EnumerateObject())
            {
                if (string.Equals(property.Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    value = property.Value;
                    return true;
                }
            }
        }

        value = default;
        return false;
    }
}
