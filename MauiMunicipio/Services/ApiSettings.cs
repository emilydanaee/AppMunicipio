namespace MauiMunicipio.Services;

public static class ApiSettings
{
    private const string BaseUrlKey = "api_base_url";

    public static string BaseUrl
    {
        get
        {
            var saved = Preferences.Default.Get(BaseUrlKey, string.Empty);
            return string.IsNullOrWhiteSpace(saved) ? GetDefaultUrl() : Normalize(saved);
        }
        set => Preferences.Default.Set(BaseUrlKey, Normalize(value));
    }

    public static void Reset() => Preferences.Default.Remove(BaseUrlKey);

    public static string Normalize(string value)
    {
        var normalized = (value ?? string.Empty).Trim();
        if (string.IsNullOrWhiteSpace(normalized))
            return GetDefaultUrl();

        if (!normalized.StartsWith("http://", StringComparison.OrdinalIgnoreCase) &&
            !normalized.StartsWith("https://", StringComparison.OrdinalIgnoreCase))
        {
            normalized = "http://" + normalized;
        }

        normalized = normalized.TrimEnd('/');
        if (normalized.EndsWith("/swagger", StringComparison.OrdinalIgnoreCase))
            normalized = normalized[..^8].TrimEnd('/');
        if (normalized.EndsWith("/api", StringComparison.OrdinalIgnoreCase))
            normalized = normalized[..^4].TrimEnd('/');

        return normalized;
    }

    private static string GetDefaultUrl()
    {
#if ANDROID
        return "http://10.0.2.2:5279";
#else
        return "http://localhost:5279";
#endif
    }
}
